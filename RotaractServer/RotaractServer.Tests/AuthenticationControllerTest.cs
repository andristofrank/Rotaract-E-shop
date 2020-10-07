using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RotaractServer.Models;
using RotaractServer.Services;
using RotaractServer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using RotaractServer.Data;
using System.Data.Entity;

namespace RotaractServer.Tests
{
    [TestClass]
    public class AuthenticationControllerTest
    {
        [TestMethod]
        public void Register_ShouldRegisterNewUserER()
        {
            var mockUser = new List<User>
            {
                new User
                {
                    UserId = 112,
                    Username = "Frankie",
                    FirstName = "Frank",
                    LastName = "Sinatra",
                    DistrictNumber = 1,
                    Email = "frank.sinatra@myway.com",
                    Password = "newyorknewyork",
                    Role = "Customer"
                }
            };

            var districtNo = new List<DistrictNo>
            {
                new DistrictNo
                {
                    DistrictNumber = 0
                },
                new DistrictNo
                {
                    DistrictNumber = 1
                }
            };

            var context = new Mock<RotaractServrerContext>();

            var mockDbSet = new Mock<DbSet<User>>();
            mockDbSet.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new User()));

            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.Provider)
                .Returns(mockUser.AsQueryable().Provider);
            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.Expression)
                .Returns(mockUser.AsQueryable().Expression);
            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.ElementType)
                .Returns(mockUser.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<User>>()
                .Setup(m => m.GetEnumerator())
                .Returns(mockUser.GetEnumerator());

            mockDbSet.Setup(m => m.Add(It.IsAny<User>()))
                .Callback<User>((entity) => mockUser.Add(entity));
            context.Setup(c => c.Users)
                .Returns(mockDbSet.Object);

            var mockDbSet2 = new Mock<DbSet<DistrictNo>>();
            mockDbSet2.Setup(s => s.FindAsync(It.IsAny<int>())).Returns(Task.FromResult(new DistrictNo()));

            mockDbSet2.As<IQueryable<DistrictNo>>()
                .Setup(m => m.Provider)
                .Returns(mockUser.AsQueryable().Provider);
            mockDbSet2.As<IQueryable<DistrictNo>>()
                .Setup(m => m.Expression)
                .Returns(mockUser.AsQueryable().Expression);
            mockDbSet2.As<IQueryable<DistrictNo>>()
                .Setup(m => m.ElementType)
                .Returns(mockUser.AsQueryable().ElementType);
            mockDbSet2.As<IQueryable<DistrictNo>>()
                .Setup(m => m.GetEnumerator())
                .Returns(districtNo.GetEnumerator());

            mockDbSet2.Setup(m => m.Add(It.IsAny<DistrictNo>()))
                .Callback<DistrictNo>((entity) => districtNo.Add(entity));
            context.Setup(c => c.DistrictNos)
                .Returns(mockDbSet2.Object);
            //Arrange
            var repoDataAccess = new RotaractRepository(context.Object);

            var userExpected = new User
            {
                UserId = 112,
                Username = "Frankie",
                FirstName = "Frank",
                LastName = "Sinatra",
                DistrictNumber = 1,
                Email = "frank.sinatra@myway.com",
                Password = "newyorknewyork",
                Role = "Customer"
            };
            //Act
            repoDataAccess.AddUser(userExpected);

            //Assert
            Assert.IsTrue(mockUser.Any(u => u.DistrictNumber == 1));
            Assert.AreEqual(mockUser[0].UserId, userExpected.UserId);
        }
    }
}
