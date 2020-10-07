using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Web.Http;
using RotaractServer.Data;
using RotaractServer.Helpers;
using RotaractServer.Models;
using RotaractServer.Models.DAO_Models;
using RotaractServer.ResourseParameters;
using RotaractServer.Services;
using WebApi.OutputCache.V2;

namespace RotaractServer.Controllers.API
{
    [RoutePrefix("api")]
    public class OrderController : ApiController
    {
        /// <summary>
        /// This is AppDbContext instance.
        /// </summary>
        private readonly IRotaractRepository _rotaractRepository;

        public OrderController(IRotaractRepository repo)
        {
            this._rotaractRepository = repo;
        }


        /// <summary>
        /// Retrieves all of the orders from the database based on the status
        /// </summary>
        /// <returns>
        /// Returns a list of type orders with the past status
        /// </returns>
        /// <example>
        /// <code>
        /// donations = context.Donations.ToList();
        /// </code>
        /// </example>
        [Authorize(Roles = "Administrator")]
        [HttpGet]
        [Route("order/{status}")]
        //[CacheOutput(ClientTimeSpan = 30, ServerTimeSpan = 30)]
        public HttpResponseMessage GetOrdersByStatus(string status, [FromUri] PageResourseParameters pageResourceParameters)
        {
            try
            {
                var ordersFromRepo = _rotaractRepository.GetAllOrdersByStatus(status,pageResourceParameters);

                var paginationMetadata = new
                {
                    totalCount = ordersFromRepo.TotalCount,
                    pageSize = ordersFromRepo.PageSize,
                    currentPage = ordersFromRepo.CurrentPage,
                    totalPages = ordersFromRepo.TotalPages
                };

                var shapedOrders = ordersFromRepo
                    .ShapeData(pageResourceParameters.Fields);

                var response = Request.CreateResponse(HttpStatusCode.OK, shapedOrders);
                response.Headers.Add("x-pagination",
                    JsonSerializer.Serialize(paginationMetadata));
                response.Headers.Add("Access-Control-Expose-Headers", "x-pagination");
                return response;
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

      


        /// <summary>
        /// Places New Order in the database
        /// </summary>
        /// <remarks>
        /// Sample Order:
        ///
        ///
        ///     {
        ///        "OrderItems": [
        ///          {
        ///            "product": {
        ///                  "ProductId": 0
        ///                 },
        ///             "Quantity": 0
        ///          }
        ///         ],
        ///         "ShippingDetailsModel":
        ///             {
        ///               "Country": "string",
        ///               "City": "string",
        ///               "AddressLine": "string",
        ///               "CountyOrRegion": "string",
        ///               "PostalCode": 0,
        ///               "FirstName": "string",
        ///               "LastName": "string",
        ///               "PhoneNumber": 0
        ///             },
        ///            "TotalPrice": 0
        ///           }
        ///
        /// </remarks>
        /// <returns>
        /// A list of type donations with all the donations in the database
        /// </returns>
        [Authorize]
        [HttpPost]
        [Route("order")]
        public HttpResponseMessage PostNewOrder([FromBody]OrderDto orderDto)
        {
            try
            { 
                var identity = (ClaimsIdentity)User.Identity;
                var userId = Int32.Parse(identity.Claims.FirstOrDefault(c => c.Type == "UserId").Value); 


                //Check method if model is correct first
                _rotaractRepository.PostNewOrder(orderDto,userId);
                _rotaractRepository.Save();


                return Request.CreateResponse(HttpStatusCode.Accepted, "New order added");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }


        /// <summary>
        /// Updates the status of a specified order using the Order Id
        /// </summary>
        /// <remarks>
        /// Sample Order:
        ///
        ///     {
        ///        "OrderId": 0,
        ///        "Status": "CANCELLED/PENDING/CONFIRMED/INPROGRESS/SHIPPING/COMPLETED"
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// A list of type donations with all the donations in the database
        /// </returns>
        [Authorize(Roles = "Administrator")]
        [HttpPut]
        [Route("order")]
        public HttpResponseMessage UpdateStatus(OrderDto orderDto)
        {
            try
            {
                if (_rotaractRepository.UpdateStatusOfOrder(orderDto))
                {
                    _rotaractRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Status successfully updated ");
                }

                return Request.CreateResponse(HttpStatusCode.Forbidden, "Unable to Update to selected Status");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}
