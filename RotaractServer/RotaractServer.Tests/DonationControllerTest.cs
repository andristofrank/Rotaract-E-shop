using System;
using System.Web.Http;
using RotaractServer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RotaractServer.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Web.Http.Validation;
using RotaractServer.Areas.HelpPage.ModelDescriptions;
using System.Web.Http.ModelBinding;
using RotaractServer.ResourseParameters;
using RotaractServer.Helpers;
using RotaractServer.Models;
using AutoMapper;
using System.Linq;
using RotaractServer.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using RotaractServer.Models.DAO_Models;

namespace RotaractServer.Tests
{
    [TestClass]
    public class DonationControllerTest
    {
        [TestMethod]
        public void GetActiveDonations_ShouldReturnAllActiveDonationPrograms()
        {
            //expected
            var expected = new List<Models.DonationProgram>()
            {
                new Models.DonationProgram {
                    DonationProgramId = 23,
                    DonationProgramName = "Dona",
                    Description = "Maxim",
                    ImageRef = "an image ref",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 12
                },
                new Models.DonationProgram {
                    DonationProgramId = 24,
                    DonationProgramName = "Donna",
                    Description = "Maxxim",
                    ImageRef = "an image refs",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow,
                    Total = 13
                }
            };

            //mock repository
            var mockRepo = new Mock<IRotaractRepository>();
            mockRepo.Setup(d => d.GetActiveDonationPrograms())
                .Returns(expected);

            //Arrange
            DonationController donationController = new DonationController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage() { },
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = donationController.GetActiveDonations();

            //Assert
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.TryGetContentValue<List<Models.DonationProgram>>(out expected));
        }

        [TestMethod]
        public void GetDonations_ShouldReturnPaginatedListOfDonationPrograms()
        {
            //expected
            var expected = new List<DonationProgram>()
            {
                new DonationProgram {
                    DonationProgramId = 1,
                    DonationProgramName = "One",
                    Description = "Maxim one",
                    ImageRef = "An image ref",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 111
                },
                new DonationProgram
                {
                    DonationProgramId = 2,
                    DonationProgramName = "Two",
                    Description = "Maxim two",
                    ImageRef = "An image refs",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 112
                }
            };

            //mock repo with pagination
            var pageParameters = new PageResourseParameters();
            var mockRepo = new Mock<IRotaractRepository>();

            var mockList = new PagedList<DonationProgram>(expected, 2, 1, 1);

            mockRepo.Setup(d => d.GetDonationPrograms(pageParameters)).Returns(mockList);

            //Arrange
            DonationController donationController = new DonationController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = donationController.GetDonations(pageParameters);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Headers.Contains("x-pagination"));
        }
        [TestMethod]
        public void DeleteDonationProgram_ShouldDeleteTheDonationProgramER()
        {
            var mockDonationPrograms = new List<DonationProgram>()
            {
                new DonationProgram {
                    DonationProgramId = 1,
                    DonationProgramName = "One",
                    Description = "Maxim one",
                    ImageRef = "An image ref",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 111
                },
                new DonationProgram
                {
                    DonationProgramId = 2,
                    DonationProgramName = "Two",
                    Description = "Maxim two",
                    ImageRef = "An image refs",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 112
                },
                new DonationProgram
                {
                    DonationProgramId = 3,
                    DonationProgramName = "Three",
                    Description = "Maxim three",
                    ImageRef = "An image refss",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 113
                }
            };

            var mockDbSet = new Mock<DbSet<DonationProgram>>();
            var context = new Mock<RotaractServrerContext>();

            mockDbSet.Setup(s => s.FindAsync(It.IsAny<Guid>())).Returns(Task.FromResult(new DonationProgram()));

            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.Provider)
                .Returns(mockDonationPrograms.AsQueryable().Provider);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.Expression)
                .Returns(mockDonationPrograms.AsQueryable().Expression);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.ElementType)
                .Returns(mockDonationPrograms.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.GetEnumerator())
                .Returns(mockDonationPrograms.GetEnumerator());

            mockDbSet.Setup(m => m.Remove(It.IsAny<DonationProgram>()))
                .Callback<DonationProgram>((entity) => mockDonationPrograms.Remove(entity));
            context.Setup(c => c.DonationPrograms)
                .Returns(mockDbSet.Object);

            var repoDonationProgramDataAccess = new RotaractRepository(context.Object);
            int idToDelete = 1;
            repoDonationProgramDataAccess.DeleteDonationProgram(idToDelete);

            //context.VerifyGet(x => x.DonationPrograms, Times.Exactly(2));
            //context.Verify(x => x.SaveChanges(), Times.Once());

            //Asserts
            Assert.AreEqual(mockDonationPrograms.Count, 2);
            Assert.IsFalse(mockDonationPrograms.Any(x => x.DonationProgramId == idToDelete));
        }

        [TestMethod]
        public void DeleteDonationProgram_ShouldDeleteTheDonationProgramREPO()
        {
            //Unit test inits
            var mockRepo = new Mock<IRotaractRepository>();
            const int IdToDelete = 1;

            //Arrange
            DonationController donationController = new DonationController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage() { },
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = donationController.DeletedDonation(IdToDelete);

            //Verify
            mockRepo.Verify(v => v.DeleteDonationProgram(IdToDelete), Times.Once());

            //Assert
            Assert.IsFalse(result.IsSuccessStatusCode);
        }

        [TestMethod]
        public void MakeDonation_ShouldAllowAnUserToDonateREPO()
        {
            //declare and init
            var mockDonationProgram = new DonationProgram
            {
                DonationProgramId = 1,
                DonationProgramName = "One",
                Description = "Maxim one",
                ImageRef = "An image ref",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Total = 111
            };
            const int AmountToDonate = 20;
            const int UserID = 1234;

            var mockExpected = new DonationDto()
            {
                donationProgram = mockDonationProgram,
                amount = AmountToDonate
            };

            var mockRepo = new Mock<IRotaractRepository>();

            mockRepo.Setup(d => d.Donate(mockExpected, UserID));

            //Arrange
            DonationController donationController = new DonationController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = donationController.MakeDonation(mockExpected, UserID);

            //Assert
            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
        }

        [TestMethod]
        public void MakeDonation_ShouldAllowAnUserToDonateER()
        {
            //Arrange
            var mockExpected = new List<DonationProgram>()
            {
                new DonationProgram
                {
                    DonationProgramId = 1,
                    DonationProgramName = "One",
                    Description = "Maxim one",
                    ImageRef = "An image ref",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 111
                },
            };

            var mockDonationsExpected = new List<Donation>()
            {
                new Donation
                {
                    DonationId = 1,
                    UserId = 1234,
                    DonaitonProgram_auditId = 1,
                    Amount = 20
                },
            };

            var mockAuditExpected = new List<DonationProgram_Audit>()
            {
                new DonationProgram_Audit
                {
                    DonationProgram_AuditId = 1,
                    DonationProgramId = 1,
                    DonationProgramName = "One",
                    Description = "Maxim one",
                    ImageRef = "An image ref",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 131,
                    Updated_at = DateTime.Now,
                    Operation = "UPDATED"
                },
            };

            //refer to a mock of the DbContext
            var context = new Mock<RotaractServrerContext>();

            //mock the DB tables respecting the flow of data
            #region DonationProgram
            var mockDbSet = new Mock<DbSet<DonationProgram>>();
            mockDbSet.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new DonationProgram()));

            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.Provider)
                .Returns(mockExpected.AsQueryable().Provider);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.Expression)
                .Returns(mockExpected.AsQueryable().Expression);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.ElementType)
                .Returns(mockExpected.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.GetEnumerator())
                .Returns(mockExpected.GetEnumerator());

            mockDbSet.Setup(m => m.Add(It.IsAny<DonationProgram>()))
                .Callback<DonationProgram>((entity) => mockExpected.Add(entity));
            context.Setup(c => c.DonationPrograms)
                .Returns(mockDbSet.Object);
            #endregion

            #region DonationProgram_Audit
            var mockDbSet2 = new Mock<DbSet<DonationProgram_Audit>>();
            mockDbSet2.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new DonationProgram_Audit()));

            mockDbSet2.As<IQueryable<DonationProgram_Audit>>()
                .Setup(m => m.Provider)
                .Returns(mockExpected.AsQueryable().Provider);
            mockDbSet2.As<IQueryable<DonationProgram_Audit>>()
                .Setup(m => m.Expression)
                .Returns(mockExpected.AsQueryable().Expression);
            mockDbSet2.As<IQueryable<DonationProgram_Audit>>()
                .Setup(m => m.ElementType)
                .Returns(mockExpected.AsQueryable().ElementType);
            mockDbSet2.As<IQueryable<DonationProgram_Audit>>()
                .Setup(m => m.GetEnumerator())
                .Returns(mockAuditExpected.GetEnumerator());

              mockDbSet2.Setup(m => m.Add(It.IsAny<DonationProgram_Audit>()))
                .Callback<DonationProgram_Audit>((entity) => mockAuditExpected.Add(entity));
            context.Setup(c => c.DonationProgramAudits)
                .Returns(mockDbSet2.Object);
            #endregion

            #region Donation
            var mockDbSet3 = new Mock<DbSet<Donation>>();
            mockDbSet3.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new Donation()));

            mockDbSet3.As<IQueryable<Donation>>()
                .Setup(m => m.Provider)
                .Returns(mockExpected.AsQueryable().Provider);
            mockDbSet3.As<IQueryable<Donation>>()
                .Setup(m => m.Expression)
                .Returns(mockExpected.AsQueryable().Expression);
            mockDbSet3.As<IQueryable<Donation>>()
                .Setup(m => m.ElementType)
                .Returns(mockExpected.AsQueryable().ElementType);
            mockDbSet3.As<IQueryable<Donation>>()
                .Setup(m => m.GetEnumerator())
                .Returns(mockDonationsExpected.GetEnumerator());

            mockDbSet3.Setup(m => m.Add(It.IsAny<Donation>()))
                .Callback<Donation>((entity) => mockDonationsExpected.Add(entity));
            context.Setup(c => c.Donations)
                .Returns(mockDbSet3.Object);
            #endregion

            //refer to the the repo which takes a reference to the mock of the Db Context
            var repoDonationDataAccess = new RotaractRepository(context.Object);

            //final setup of data for calling the repo method and analyse the results
            const int AmountToDonate = 20;
            const int UserID = 1234;
            var DonationDto = new DonationDto
            {
                donationProgram = mockExpected[0],
                amount = AmountToDonate
            };

            //Act
            repoDonationDataAccess.Donate(DonationDto, UserID);

            //Asserts
            Assert.IsTrue(mockExpected.Any(x => x.Total == 131));
        }

        [TestMethod]
        public void AddDonationProgram_ShouldAddNewDonationProgramREPO()
        {
            var mockDonationProgramsDto = new DonationProgramDto()
            {
                DonationProgramId = 1,
                DonationProgramName = "EndPolio",
                Description = "End Polio Now!",
                ImageRef = "Ref",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Total = 100
            };

            var mockExpected = new DonationProgram()
            {
                DonationProgramId = mockDonationProgramsDto.DonationProgramId,
                DonationProgramName = mockDonationProgramsDto.DonationProgramName,
                Description = mockDonationProgramsDto.Description,
                ImageRef = mockDonationProgramsDto.ImageRef,
                StartDate = mockDonationProgramsDto.StartDate,
                EndDate = mockDonationProgramsDto.EndDate,
                Total = mockDonationProgramsDto.Total
            };

            var mockRepo = new Mock<IRotaractRepository>();

            mockRepo.Setup(d => d.AddDonationProgram(mockExpected));

            //Arrange
            DonationController donationController = new DonationController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = donationController.AddDonationProgram(mockDonationProgramsDto);

            //Assert
            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);
        }

        [TestMethod]
        public void AddDonationProgram_ShouldAddNewDonationProgramER()
        {
            var mockDonationProgramList = new List<DonationProgram>()
            {
                new DonationProgram
                {
                    DonationProgramId = 1,
                    DonationProgramName = "One",
                    Description = "Maxim one",
                    ImageRef = "An image ref",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 111
                },
                new DonationProgram
                {
                    DonationProgramId = 2,
                    DonationProgramName = "Two",
                    Description = "Maxim two",
                    ImageRef = "An image refs",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Total = 100
                }
            };

            var context = new Mock<RotaractServrerContext>();

            #region MockDbSetup DonationProgram
            var mockDbSet = new Mock<DbSet<DonationProgram>>();
            mockDbSet.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new DonationProgram()));

            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.Provider)
                .Returns(mockDonationProgramList.AsQueryable().Provider);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.Expression)
                .Returns(mockDonationProgramList.AsQueryable().Expression);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.ElementType)
                .Returns(mockDonationProgramList.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<DonationProgram>>()
                .Setup(m => m.GetEnumerator())
                .Returns(mockDonationProgramList.GetEnumerator());

            mockDbSet.Setup(m => m.Add(It.IsAny<DonationProgram>()))
                .Callback<DonationProgram>((entity) => mockDonationProgramList.Add(entity));
            context.Setup(c => c.DonationPrograms)
                .Returns(mockDbSet.Object);
            #endregion

            var repoDonationDataAccess = new RotaractRepository(context.Object);

            var donationProgram = new DonationProgram
            {
                DonationProgramId = 3,
                DonationProgramName = "Three",
                Description = "Maxim three",
                ImageRef = "An image refsss",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Total = 333
            };

            repoDonationDataAccess.AddDonationProgram(donationProgram);

            //Asserts
            Assert.IsTrue(mockDonationProgramList.Any(x => x.DonationProgramId == 3));
            Assert.IsTrue(mockDonationProgramList.Any(x => x.DonationProgramName.Equals("Three")));
            Assert.IsTrue(mockDonationProgramList.Any(x => x.Total == 0));
        }

        [TestMethod]
        public void UpdateDonationProgram_ShouldUpdateDonationProgramREPO()
        {
            var mockDonationProgramsDto = new DonationProgramDto()
            {
                DonationProgramId = 1,
                DonationProgramName = "EndPolio",
                Description = "End Polio Now!",
                ImageRef = "Ref",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Total = 100
            };

            var mockExpected = new DonationProgram()
            {
                DonationProgramId = mockDonationProgramsDto.DonationProgramId,
                DonationProgramName = mockDonationProgramsDto.DonationProgramName,
                Description = mockDonationProgramsDto.Description,
                ImageRef = mockDonationProgramsDto.ImageRef,
                StartDate = mockDonationProgramsDto.StartDate,
                EndDate = mockDonationProgramsDto.EndDate,
                Total = mockDonationProgramsDto.Total
            };

            var mockExpectedUpdated = new DonationProgramDto()
            {
                DonationProgramId = 1,
                DonationProgramName = "EndPolioNow",
                Description = "End Polio Now!",
                ImageRef = "Ref",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                Total = 1000
            };

            var mockRepo = new Mock<IRotaractRepository>();

            mockRepo.Setup(d => d.UpdateDonation(mockExpected));

            //Arrange
            DonationController donationController = new DonationController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = donationController.UpdateDonation(mockExpectedUpdated);

            //Assert
            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
        }
    }
}
