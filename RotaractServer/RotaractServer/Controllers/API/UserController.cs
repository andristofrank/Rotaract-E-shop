using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Internal;
using Microsoft.Owin.Security.OAuth;
using RotaractServer.Authorization;
using RotaractServer.Data;
using RotaractServer.Helpers;
using RotaractServer.Models;
using RotaractServer.ResourseParameters;
using RotaractServer.Services;
using WebApi.OutputCache.V2;

namespace RotaractServer.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {

        private readonly IRotaractRepository _rotaractRepository;

        public UserController(IRotaractRepository repo)
        {
            this._rotaractRepository = repo;
        }

        /// <summary>
        /// Getting User Details based on the Authorized Token.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("details")]
        [HttpGet]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage GetUserDetails()
        {
            try
            {
                    var identity = (ClaimsIdentity)User.Identity;  var userId = Int32.Parse(identity.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                    var userDetailsFromRepo = _rotaractRepository.GetUserDetails(userId);

                    if (userDetailsFromRepo == null) return Request.CreateResponse(HttpStatusCode.BadRequest, "Account doesn't exist");

                    return Request.CreateResponse(HttpStatusCode.OK, userDetailsFromRepo);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Error Occurred when loading Account Identity");
            }
        }


        /// <summary>
        /// Retrieves all orders accosiated with the user
        /// </summary>
        /// <returns>
        /// A list of type donations with all the donations in the database
        /// </returns>
        /// <example>
        /// <code>
        /// donations = context.Donations.ToList();
        /// </code>
        /// </example>
        [EnableCors("RotaractCorsPolicy")]
        [Authorize]
        [HttpGet]
        [Route("order")]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage GetOrdersByUser([FromUri] PageResourseParameters pageResourceParameters)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                int userId = Int32.Parse(identity.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var ordersFromRepo = _rotaractRepository.GetAllOrdersById(userId,pageResourceParameters);

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
        /// Retrieves all donations accosiated with the user
        /// </summary>
        /// <returns>
        /// A list of type donations with all the donations in the database
        /// </returns>
        /// <example>
        /// <code>
        /// donations = context.Donations.ToList();
        /// </code>
        /// </example>
        [EnableCors("RotaractCorsPolicy")]
        [Authorize]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("donation")]
        [CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage GetDonationsByUser([FromUri] PageResourseParameters pageResourceParameters)
        {
            try
            {
                var identity = (ClaimsIdentity)User.Identity;
                int userId = Int32.Parse(identity.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var donationProgramsFromRepo = _rotaractRepository.GetDonationProgramsById(userId,pageResourceParameters);

                var paginationMetadata = new
                {
                    totalCount = donationProgramsFromRepo.TotalCount,
                    pageSize = donationProgramsFromRepo.PageSize,
                    currentPage = donationProgramsFromRepo.CurrentPage,
                    totalPages = donationProgramsFromRepo.TotalPages
                };

                IEnumerable<DonationProgramDto> donationProgramDtos = new List<DonationProgramDto>();
                AutoMapper.Mapper.Map(donationProgramsFromRepo, donationProgramDtos);

                var shapedDonations = donationProgramDtos
                    .ShapeData(pageResourceParameters.Fields);

                var response = Request.CreateResponse(HttpStatusCode.OK, shapedDonations);
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
        /// Email with authorized link is sent to the user via email.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("forgotPassword/{username}")]
        [HttpGet]
        public HttpResponseMessage ForgotPassword(string username)
        {
            return Request.CreateResponse(HttpStatusCode.NotImplemented, "Feature not yet implemented");
        }
    }
}
