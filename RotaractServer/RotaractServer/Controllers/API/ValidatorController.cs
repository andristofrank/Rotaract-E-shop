using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNetCore.Cors;
using RotaractServer.Data;
using RotaractServer.ResourseParameters;
using RotaractServer.Services;
using WebApi.OutputCache.V2;

namespace RotaractServer.Controllers
{
    /*
    The main ValidatorController class
    Contains all methods for performing basic Validation functions
    */
    /// <summary>
    /// The main <c>Math</c> class.
    /// Contains all methods for performing basic math functions.
    /// <list type="bullet">
    /// <item>
    /// <term>CheckDistrictNumber</term>
    /// <description>Checks if a given district number matches one in the database</description>
    /// </item>
    /// <item>
    /// <term>ValidateUserName</term>
    /// <description>Checks if a given username matches one in the database</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can check a district number and a username.</para>
    /// </remarks>
    [RoutePrefix("validator")]
    public class ValidatorController : ApiController
    {

        private readonly IRotaractRepository _rotaractRepository;

        public ValidatorController(IRotaractRepository repo)
        {
            this._rotaractRepository = repo;
        }


        /// <summary>
        /// Retrieves a district number and returns a true or false based on whether it exists
        /// </summary>
        /// <remarks>
        /// Sample Product:
        ///
        ///     {
        ///        "DistrictNumber": 0
        ///     }
        ///
        /// </remarks>
        /// <param number="districtNumber">A number referencing a specific reference number in the database.</param>
        [Route("districtNumber")]
        [HttpPost]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage CheckDistrictNumber([FromBody]ValidatorResourceParameters parameter)
        {
            try
            {
                if (!_rotaractRepository.DistrictNumberExists(parameter.DistrictNumber)) return Request.CreateResponse(HttpStatusCode.OK, false);

                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Retrieves a username and returns a true or false based on whether it exists
        /// </summary>
        /// <remarks>
        /// Sample Product:
        ///
        ///     {
        ///         "UserName": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="UserName"></param>
        [Route("username")]
        [HttpPost]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage ValidateUserName([FromBody]ValidatorResourceParameters parameter)
        {
            try
            {
                if (!_rotaractRepository.UserNameExists(parameter.UserName)) return Request.CreateResponse(HttpStatusCode.OK, false);

                return Request.CreateResponse(HttpStatusCode.Accepted, true);
            
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Retrieves an email and returns a true or false based on whether it exists
        /// </summary>
        /// <remarks>
        /// Sample Product:
        ///
        ///     {
        ///       "Email": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [Route("email")]
        [HttpPost]
        public HttpResponseMessage ValidateEmail([FromBody]ValidatorResourceParameters parameter)
        {
            try
            {
                if (!_rotaractRepository.EmailExists(parameter.Email)) return Request.CreateResponse(HttpStatusCode.OK, false);

                return Request.CreateResponse(HttpStatusCode.Accepted, true);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}
