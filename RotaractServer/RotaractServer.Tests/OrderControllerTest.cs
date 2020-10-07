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
using RotaractServer.Controllers.API;

namespace RotaractServer.Tests
{
    [TestClass]
    public class OrderControllerTest
    {
        [TestMethod]
        public void GetOrdersByStatus_ShouldRetrieveOrdersByStatus()
        {
            var user = new List<User>
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

            var shippingDetails = new List<ShippingDetailsDto>
            {
                new ShippingDetailsDto
                {
                    Country = "USA",
                    City = "San Francisco",
                    PostalCode = 911,
                    AddressLine = "Californication RHCP",
                    CountyOrRegion = "California",
                    FirstName = "Frank",
                    LastName = "Sinatra",
                    PhoneNumber = 12345678
                }
            };

            var product = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "Microphone",
                    Description = "Rotaract personalized microphone",
                    ImageRef = "img ref",
                    Inventory = 5,
                    Price = 1250
                }
            };

            var orderItems = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    product = product[0],
                    Quantity = 1
                }
            };

            var orders = new List<OrderDto>
            {
                new OrderDto
                {
                    UserId = user[0].UserId,
                    OrderId = 2,
                    OrderItems = orderItems,
                    ShippingDetailsModel = shippingDetails[0],
                    DateStamp = "01.12.2020",
                    Status = "PENDING",
                    TotalPrice = orderItems[0].product.Price * orderItems[0].Quantity
                }
            };

            //mock repo with pagination
            var pageParameters = new PageResourseParameters();
            var mockRepo = new Mock<IRotaractRepository>();

            var mockList = new PagedList<OrderDto>(orders, 1, 1, 1);
            const string status = "PENDING";

            mockRepo.Setup(o => o.GetAllOrdersByStatus(status, pageParameters)).Returns(mockList);

            //Arrange
            OrderController orderController = new OrderController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            //Act
            var result = orderController.GetOrdersByStatus(status, pageParameters);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(HttpResponseMessage));
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
            Assert.IsTrue(result.Headers.Contains("x-pagination"));
        }

        [TestMethod]
        public void UpdateStatus_ShouldUpdateTheStatusOfAnOrder()
        {
            //Arange
            //Reproduce data required
            var productList = new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    Name = "T-shirt",
                    Description = "Rotaract personalized t-shirt",
                    ImageRef = "Ref",
                    Inventory = 10,
                    Price = 8
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Pants",
                    Description = "Rotaract personalized pants",
                    ImageRef = "Ref",
                    Inventory = 50,
                    Price = 10
                }
            };

            var shippingDetails = new ShippingDetailsDto
            {
                Country = "Denmark",
                City = "Horsens",
                PostalCode = 8700,
                CountyOrRegion = "Midtjylland",
                AddressLine = "Kirkegaardvej 777",
                FirstName = "Max",
                LastName = "Maximovic",
                PhoneNumber = 91123456
            };

            var orderItemDtoList = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    product = productList[0],
                    Quantity = 5
                },
                new OrderItemDto
                {
                    product = productList[1],
                    Quantity = 2
                }
            };

            var orderDto = new OrderDto
            {
                OrderId = 1,
                UserId = 10,
                OrderItems = orderItemDtoList,
                ShippingDetailsModel = shippingDetails,
                DateStamp = "2020-05-25",
                Status = "CONFIRMED",
                TotalPrice = (orderItemDtoList[0].product.Price * orderItemDtoList[0].Quantity)
                            + (orderItemDtoList[1].product.Price * orderItemDtoList[1].Quantity)
            };

            var mockRepo = new Mock<IRotaractRepository>();
            mockRepo.Setup(d => d.UpdateStatusOfOrder(orderDto));

            //Arrange
            OrderController orderController = new OrderController(mockRepo.Object)
            {
                Request = new System.Net.Http.HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };

            var orderDtoUpdated = new OrderDto
            {
                OrderId = 1,
                UserId = 10,
                OrderItems = orderItemDtoList,
                ShippingDetailsModel = shippingDetails,
                DateStamp = "2020-05-25",
                Status = "INPROGRESS",
                TotalPrice = (orderItemDtoList[0].product.Price * orderItemDtoList[0].Quantity)
                            + (orderItemDtoList[1].product.Price * orderItemDtoList[1].Quantity)
            };

            //Act
            var result = orderController.UpdateStatus(orderDtoUpdated);

            //Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }

    }
}
