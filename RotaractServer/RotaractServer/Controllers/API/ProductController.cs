using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RotaractServer.App_Start;
using RotaractServer.Controllers.API;
using RotaractServer.Data;
using RotaractServer.Helpers;
using RotaractServer.Models;
using RotaractServer.ResourseParameters;
using RotaractServer.Services;
using Swashbuckle.Examples;
using Swashbuckle.Swagger;
using WebApi.OutputCache.V2;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace RotaractServer.Controllers
{
    /*
     The main ProductsController class
     Contains all methods for performing accessing and manipulating entries.
     */
    /// <summary>
    /// The main <c>ProductsController</c> class.
    /// Contains all methods for performing basic CRUD functions.
    /// <list type="bullet">
    /// <item>
    /// <term>GetProducts</term>
    /// <description>Retrieves all the data from the database.</description>
    /// </item>
    /// <item>
    /// <term>GetProductsToPagination</term>
    /// <description>Retrieves all the products based on the page the user is on.</description>
    /// </item>
    /// <item>
    /// <term>AddProduct</term>
    /// <description>Add a product into the database</description>
    /// </item>
    /// <item>
    /// <term>UpdateProduct</term>
    /// <description>Updates the values in the database on a specific product</description>
    /// </item>
    /// <item>
    /// <term>DeleteProduct</term>
    /// <description>Based on product id, deletes an entry in the database.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can Add, Update, Delete and Get all the products in the database.</para>
    /// </remarks>
    [System.Web.Http.RoutePrefix("api")]
    public class ProductsController : ApiController
    {
        /// <summary>
        /// This is AppDbContext instance.
        /// </summary>

        private readonly IRotaractRepository _rotaractRepository;
        private readonly IPropertyMappingService _propertyMappingService = new ProductPropertyMappingService();

        public ProductsController(IRotaractRepository repo)
        {
            this._rotaractRepository = repo;
        }

        /// <summary>
        /// Retrieves a specific amount of products from the database using Paging 
        /// </summary>
        /// <returns>
        /// A list of type products with a specified Length.
        /// </returns>
        [EnableCors("RotaractCorsPolicy")]
        [HttpGet]
        [Route("product")]
        //[CacheOutput(ClientTimeSpan = 30, ServerTimeSpan = 30)]
        public HttpResponseMessage GetProducts([FromUri] PageResourseParameters pageResourceParameters)
        {
            try
            {

                var productsFromRepo = _rotaractRepository.GetProducts(pageResourceParameters);

                var paginationMetadata = new
                {
                    totalCount = productsFromRepo.TotalCount,
                    pageSize = productsFromRepo.PageSize,
                    currentPage = productsFromRepo.CurrentPage,
                    totalPages = productsFromRepo.TotalPages
                };


                IEnumerable<ProductsDto> products = new List<ProductsDto>();
                AutoMapper.Mapper.Map(productsFromRepo, products);

                var shapedProducts = products
                    .ShapeData(pageResourceParameters.Fields);
                    

                var response = Request.CreateResponse(HttpStatusCode.OK, shapedProducts);
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
        /// Adding a product to the database.
        /// </summary>
        /// <remarks>
        /// Sample Product:
        ///
        ///     {
        ///        "Name": "Product Name",
        ///        "Description": "Description Of the Product",
        ///        "ImageRef": "Compressed Image",
        ///        "Price": 1,
        ///        "Inventory": 1
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Returns a string response.
        /// </returns>
        /// <response code="400">If the product doesnt match model</response>     
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("product")]
        public HttpResponseMessage AddProduct([System.Web.Http.FromBody] ProductsDto productDto)
        {
            try
            {
                Product product = new Product();
                AutoMapper.Mapper.Map(productDto, product);
                
                _rotaractRepository.AddProduct(product);
                _rotaractRepository.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted, "Product Added to the system");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }


        /// <summary>
        ///Updates a specific entry in the database using the product id
        /// </summary>
        /// <remarks>
        /// Sample Product:
        ///
        ///     {
        ///        "ProductId": 1,
        ///        "Name": "Product Name",
        ///        "Description": "Description Of the Product",
        ///        "ImageRef": "Compressed Image",
        ///        "Price": 1
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Returns a string response based on whether the update was successful.
        /// </returns>
        /// <param Product="product">The Product object from the body of the Http request.</param>
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("product")]
        public HttpResponseMessage UpdateProduct([System.Web.Http.FromBody] ProductsDto productDto)
        {
            try
            {
                Product product = new Product();
                AutoMapper.Mapper.Map(productDto, product);

                if (_rotaractRepository.UpdateProduct(product))
                {
                    _rotaractRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Product Updated in the system");
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    "Product Does not match Model or Doesn't exist ");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }


        /// <summary>
        /// Updates a specific entry in the database using the product id
        /// </summary>
        /// <remarks>
        /// Sample Product:
        ///
        ///         {
        ///                 "ProductId": 0,
        ///                 "Inventory": 1
        ///          }
        ///
        /// 
        /// </remarks>
        /// <returns>
        /// Returns a string response based on whether the update was successful.
        /// </returns>
        /// <param Product="product">The Product object from the body of the Http request.</param>
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("product/inventory")]
        public HttpResponseMessage AddInverntoryToProduct([System.Web.Http.FromBody] ProductsDto productDto)
        {
            try
            {
                Product product = new Product();
                AutoMapper.Mapper.Map(productDto, product);
                if (_rotaractRepository.AddInventory(product))
                {
                    _rotaractRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Added inventory to product.");
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "Product Doesn't Exists");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }



        /// <summary>
        ///  Deletes a specific entry in the database using the product Id 
        /// </summary>
        /// <returns>
        /// Returns a string response based on whether the delete was successful.
        /// </returns>
        /// <param number="productId">The Product object from the body of the Http request.</param>
        [System.Web.Http.Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("product/{productId}")]
        public HttpResponseMessage DeleteProduct(int productId)
        {
            try
            {
                if (_rotaractRepository.DeleteProduct(productId))
                {
                    _rotaractRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Product deleted.");
                }
                return Request.CreateResponse(HttpStatusCode.Accepted,"Product can not be deleted, Cancel Pending Orders On this Product First");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

    }
}
