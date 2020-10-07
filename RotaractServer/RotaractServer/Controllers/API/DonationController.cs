using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RotaractServer.App_Start;
using RotaractServer.Data;
using RotaractServer.Helpers;
using RotaractServer.Models;
using RotaractServer.Models.DAO_Models;
using RotaractServer.ResourseParameters;
using RotaractServer.Services;
using Swashbuckle.AspNetCore.Filters;
using WebApi.OutputCache.V2;

namespace RotaractServer.Controllers
{
    /*
     The main DonationController class
     Contains all methods for performing accessing and manipulating entries.
     */
    /// <summary>
    /// The main <c>DonationController</c> class.
    /// Contains all methods for performing basic CRUD functions.
    /// <list type="bullet">
    /// <item>
    /// <term>GetDonations</term>
    /// <description>Retrieves all the data from the database.</description>
    /// </item>
    /// <item>
    /// <term>AddDonation</term>
    /// <description>Add a donation into the database</description>
    /// </item>
    /// <item>
    /// <term>UpdateDonation</term>
    /// <description>Updates the values in the database on a specific donation</description>
    /// </item>
    /// <item>
    /// <term>DeletedDonation</term>
    /// <description>Based on donation id, deletes an entry in the database.</description>
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <para>This class can Add, Update, Delete and Get all the donations in the database.</para>
    /// </remarks>
    [RoutePrefix("api")]
    public class DonationController : ApiController
    {
        /// <summary>
        /// This is AppDbContext instance.
        /// </summary>
        private readonly IRotaractRepository _rotaractRepository;

        private readonly IPropertyMappingService _propertyMappingService = new DonationProgramPropertyMappingService();

        public DonationController(IRotaractRepository repo) 
        {
            this._rotaractRepository = repo;
        }

        /// <summary>
        /// Retrieves and returns a List of all active donation programs.
        /// </summary>
        /// <returns>
        /// A list of type donations with all the donations in the database
        /// </returns>
        /// <example>
        /// <code>
        /// donations = context.Donations.ToList();
        /// </code>
        /// </example>
        [AllowAnonymous]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("donationProgram/catalogue")]
        [CacheOutput(ClientTimeSpan = 30, ServerTimeSpan = 30)]
        public HttpResponseMessage GetActiveDonations()
        {
            try
            { 
                var donationProgramsFromRepo = _rotaractRepository.GetActiveDonationPrograms();
                
                return Request.CreateResponse(HttpStatusCode.OK, donationProgramsFromRepo);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }


         /// <summary>
        /// Retrieves and returns a List of all donation programs
        /// </summary>
        /// <returns>
        /// A list of type donations with all the donations in the database
        /// </returns>
        /// <example>
        /// <code>
        /// donations = context.Donations.ToList();
        /// </code>
        /// </example>
        [Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("donationProgram")]
        //[CacheOutput(ClientTimeSpan = 100, ServerTimeSpan = 100)]
        public HttpResponseMessage GetDonations([FromUri] PageResourseParameters pageResourceParameters)
        {
            try
            {

                var donationProgramsFromRepo = _rotaractRepository.GetDonationPrograms(pageResourceParameters);

                var paginationMetadata = new
                {
                    totalCount = donationProgramsFromRepo.TotalCount,
                    pageSize = donationProgramsFromRepo.PageSize,
                    currentPage = donationProgramsFromRepo.CurrentPage,
                    totalPages = donationProgramsFromRepo.TotalPages
                };

                var shapedDonations = donationProgramsFromRepo
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
        /// Adding a donation program to the database.
        /// </summary>
        /// <remarks>
        /// Sample Donation:
        ///
        ///     {
        ///        "DonationName": "Donation Name",
        ///        "Description": "Description Of the Donation",
        ///        "ImageRef": "Compressed Image",
        ///        "StartDate": "2020-05-14",
        ///        "EndDate": "2020-09-23"
        ///     }
        ///
        /// </remarks>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        [Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("donationProgram")]
        public HttpResponseMessage AddDonationProgram([System.Web.Http.FromBody] DonationProgramDto donationProgramDto)
        {
            try
            {
                DonationProgram donationProgram = new DonationProgram();

                _rotaractRepository.AddDonationProgram(donationProgram);
                _rotaractRepository.Save();
                return Request.CreateResponse(HttpStatusCode.Created, "Donation Added to the system");

            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        ///  Updates a specific donation program in the database using the donation program id
        /// </summary>
        /// <remarks>
        /// Sample Donation:
        ///
        ///     {
        ///        "DonationProgramId": 1,
        ///        "DonationName": "Donation Name",
        ///        "Description": "Description Of the Donation",
        ///        "ImageRef": "Compressed Image",
        ///        "StartDate": "2020-05-14",
        ///        "EndDate": "2020-09-23"
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Returns a string response based on whether the update was successful.
        /// </returns>
        /// <param Donation="donation">The donation object from the body of the Http request.</param>
        [Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpPut]
        [System.Web.Http.Route("donationProgram")]
        public HttpResponseMessage UpdateDonation([System.Web.Http.FromBody] DonationProgramDto donationProgramDto)
        {
            try
            {
                DonationProgram donationProgram = new DonationProgram();
                //AutoMapper.Mapper.Map(donationProgramDto, donationProgram);

                _rotaractRepository.UpdateDonation(donationProgram);
                _rotaractRepository.Save();
                return Request.CreateResponse(HttpStatusCode.Accepted, "Donation program updated in the system");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }


        /// <summary>
        /// Deletes a specific entry in the database using the donation program id
        /// </summary>
        /// <returns>
        /// Returns a string response based on whether the delete was successful.
        /// </returns>
        /// <param number="donationId">The Donation object from the body of the Http request.</param>
        [Authorize(Roles = "Administrator")]
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("donationProgram/{donationProgramId}")]
        public HttpResponseMessage DeletedDonation(int donationProgramId)
        {
            try
            {
                if (_rotaractRepository.DeleteDonationProgram(donationProgramId))
                {
                    _rotaractRepository.Save();
                    return Request.CreateResponse(HttpStatusCode.OK, "Donation deleted from the system");
                }

                return Request.CreateResponse(HttpStatusCode.NotFound, "Donation Program Doesn't Exist");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        /// <summary>
        /// Creates new donation to the donation program by donation program id
        /// </summary>
        /// <remarks>
        /// Sample Donation:
        ///
        ///     {
        ///        "donationProgram": {
        ///           "DonationProgramId": 1
        ///            },
        ///        "amount": 1
        ///     }
        ///
        /// </remarks>
        /// <returns>
        /// Returns a string response based on whether the update was successful.
        /// </returns>
        /// <param Donation="donation">The donation object from the body of the Http request.</param>
        [AllowAnonymous]
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("donation/{UserId}")]
        public HttpResponseMessage MakeDonation(DonationDto donationDto, int  UserId)
        {
            try
            {
                if (UserId != 0)
                {
                    _rotaractRepository.Donate(donationDto, UserId);
                }
                else
                {
                    _rotaractRepository.Donate(donationDto, 0);
                }
                _rotaractRepository.Save();

                return Request.CreateResponse(HttpStatusCode.Accepted, "Donation Made");
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
    }
}