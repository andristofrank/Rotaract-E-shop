using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using RotaractServer.App_Start;
using RotaractServer.Data;
using RotaractServer.Models;
using RotaractServer.Services;
using Swashbuckle.AspNetCore.Filters;

namespace RotaractServer.Controllers
{
    [RoutePrefix("auth")]
    public class AutheticationController : ApiController
    {

        private readonly IRotaractRepository _rotaractRepository;
        public AutheticationController(IRotaractRepository repo)
        {
            this._rotaractRepository = repo;
        }
        /// <summary>
        ///  Adding a new user to the database.
        /// </summary>
        /// <remarks>
        /// Sample User:
        ///
        ///     {
        ///         "Username": "username",
        ///         "Email": "email address",
        ///         "FirstName": "first Name",
        ///         "LastName": "last Name",
        ///         "DistrictNumber": 0,
        ///         "Password": "password",
        ///         "Role": "Customer Or Administrator"
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Returns a string response based on whether the update was successful or a possible error.
        /// </returns>
        /// <param User="user">The Product object from the body of the Http request.</param>
        [AllowAnonymous]
        [HttpPost]
        [Route("createUser")]
        public HttpResponseMessage Register([FromBody]User user)
        {
            try
            {
                if (_rotaractRepository.UserNameExists(user.Username))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username already exists");
                }
                if (_rotaractRepository.EmailExists(user.Email))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Account already exists");
                }
                if (!_rotaractRepository.DistrictNumberExists(user.DistrictNumber))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "District number not found");
                }

                _rotaractRepository.AddUser(user);
                _rotaractRepository.Save(); 

                return Request.CreateResponse(HttpStatusCode.Created, "The User has been added in the system");
                
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }

        }
    }
}