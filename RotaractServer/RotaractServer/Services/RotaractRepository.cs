using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using Microsoft.Owin.Security;
using RotaractServer.Controllers.API;
using RotaractServer.Data;
using RotaractServer.Helpers;
using RotaractServer.Models;
using RotaractServer.Models.DAO_Models;
using RotaractServer.ResourseParameters;

namespace RotaractServer.Services
{
    public class RotaractRepository : IRotaractRepository
    {
        private readonly RotaractServrerContext _context;
        public RotaractRepository(RotaractServrerContext context) 
        {
            this._context = context;
        }

        #region Products Functions
        IEnumerable<Product> IRotaractRepository.GetProducts()
        {
            return _context.Products.ToList<Product>();
        }

        public PagedList<Product> GetProducts(PageResourseParameters pageResourseParameters)
        {
            if (pageResourseParameters == null)
            {
                throw new ArgumentNullException(nameof(pageResourseParameters));
            }

            var collection = _context.Products.ToList();

            return PagedList<Product>.Create(collection,
                pageResourseParameters.PageNumber,
                pageResourseParameters.PageSize);
        }

        public Product GetProduct(int productId)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == productId);
        }

        public void AddProduct(Product product)
        {
            product.Inventory = 0;
            _context.Products.Add(product);
        }

        public bool UpdateProduct(Product product)
        {
            int productId = product.ProductId;
            var productFromRepo = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (productFromRepo != null)
            {
                productFromRepo.Description = product.Description;
                productFromRepo.ImageRef = product.ImageRef;
                productFromRepo.Name = product.Name;
                productFromRepo.Price = product.Price;
                return true;
            }
            return false;
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _context.Products.Remove(product);
        }

        public bool AddInventory(Product product)
        {
            var result = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

            if (result != null)
            {
                int NewInventory = result.Inventory + product.Inventory;

                result.Inventory = NewInventory;
                _context.SaveChanges();


                Thread t = new Thread(new ThreadStart(CheckPendingOrders));
                t.Start();

                return true;
            }
            return false;
        }

        public bool DeleteProduct(int productId)
        {
            var product = _context.Products.FirstOrDefault(n => n.ProductId == productId);

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            string query =
               " SELECT dbo.Products.ProductId , dbo.Products.Name, dbo.Products.Description, dbo.Products.ImageRef, dbo.Products.Price, dbo.Products.Inventory" +
                " FROM dbo.Products" +
                "  JOIN dbo.Products_audit" +
                "  ON dbo.Products.ProductId = dbo.Products_audit.ProductId" +
                 " JOIN dbo.OrderItems" +
                 " ON dbo.OrderItems.Product_AuditId = dbo.Products_audit.Product_AuditId" +
              "  JOIN dbo.Orders" +
                "  ON dbo.Orders.OrderId = dbo.OrderItems.OrderId" +
                " WHERE dbo.Orders.Status = 'PENDING'";

            List<Product> products = _context.Products.SqlQuery(query).ToList();

            foreach (var p in products)
            {
                if (p.ProductId == productId)
                {
                    return false;
                }
            }
            _context.Products.Remove(product);
            return true;
        }

        #endregion

        #region Donations Functions
        public PagedList<DonationProgram> GetActiveDonationPrograms(PageResourseParameters pageResourseParameters)
        {
            if (pageResourseParameters == null)
            {
                throw new ArgumentNullException(nameof(pageResourseParameters));
            }

            DateTime today = DateTime.Now;
            var collection = _context.DonationPrograms.Where(n => n.StartDate <= today && n.EndDate >= today).ToList();

            return PagedList<DonationProgram>.Create(collection,
                pageResourseParameters.PageNumber,
                pageResourseParameters.PageSize);
        }
        public List<DonationProgram> GetActiveDonationPrograms()
        {
            DateTime today = DateTime.Now;
            List<DonationProgram> donationPrograms = _context.DonationPrograms.Where(n=> n.StartDate <= today && n.EndDate >= today).ToList();
            return donationPrograms;
        }
        public PagedList<DonationProgram> GetDonationPrograms(PageResourseParameters pageResourseParameters)
        {
            if (pageResourseParameters == null)
            {
                throw new ArgumentNullException(nameof(pageResourseParameters));
            }

            // var collection = _context.Products as IQueryable<Product>;
            var collection = _context.DonationPrograms.OrderByDescending(d=> d.StartDate).ToList();

            return PagedList<DonationProgram>.Create(collection,
                pageResourseParameters.PageNumber,
                pageResourseParameters.PageSize);
        }
        public List<DonationProgram> GetDonationPrograms()
        {
            List<DonationProgram> donationPrograms = _context.DonationPrograms.ToList();
            return donationPrograms;
        }
        public PagedList<DonationProgram> GetDonationProgramsById(int UserId,
            PageResourseParameters pageResourseParameters)
        {

            if (pageResourseParameters == null)
            {
                throw new ArgumentNullException(nameof(pageResourseParameters));
            }

            //string allDonationsQuery = " SELECT dbo.DonationProgram_Audit.DonationProgramId,dbo.DonationProgram_Audit.DonationProgramName,dbo.DonationProgram_Audit.Description,dbo.DonationProgram_Audit.ImageRef,dbo.DonationProgram_Audit.StartDate,dbo.DonationProgram_Audit.EndDate,d.Amount AS Total" +
            //                " FROM dbo.DonationProgram_Audit"+
            //                " JOIN dbo.Donations d"+
            //                "    ON d.DonaitonProgram_auditId = dbo.DonationProgram_Audit.DonationProgram_AuditId"+
            //                " WHERE d.UserId = "+ UserId;

            string GroupSameDonationsQuery = "SELECT r.DonationProgramId,i.DonationProgramName,i.Description,i.ImageRef,i.StartDate,i.EndDate,r.Total "+
                                             " FROM dbo.DonationProgram_Audit i "+
                                             " JOIN( "+
                                             " SELECT  a.DonationProgramId, SUM(p.Amount) AS Total, MAX(a.Updated_at) AS Updated_at "+
                                             " FROM dbo.Donations p "+
                                             " JOIN dbo.DonationProgram_Audit a "+
                                             "    ON a.DonationProgram_AuditId = p.DonationProgram_AuditId "+
                                             " WHERE p.UserId = " + UserId +
                                             " GROUP BY a.DonationProgramId "+
                                             " ) r "+
                                             "    ON i.Updated_at = r.Updated_at "; 


            // var collection = _context.Products as IQueryable<Product>;
            var collection = _context.DonationPrograms.SqlQuery(GroupSameDonationsQuery).ToList();

            return PagedList<DonationProgram>.Create(collection,
                pageResourseParameters.PageNumber,
                pageResourseParameters.PageSize);
        }
        public void AddDonationProgram(DonationProgram donationProgram)
        {
            donationProgram.Total = 0;
            _context.DonationPrograms.Add(donationProgram);
        }
        public void UpdateDonation(DonationProgram donationProgram)
        {
            DonationProgram result = _context.DonationPrograms.SingleOrDefault(p => p.DonationProgramId == donationProgram.DonationProgramId);

            if (result != null)
            {
                result.Description = donationProgram.Description;
                result.ImageRef = donationProgram.ImageRef;
                result.DonationProgramName = donationProgram.DonationProgramName;
                result.StartDate = donationProgram.StartDate;
                result.EndDate = donationProgram.EndDate;
            }
        }
        public bool DeleteDonationProgram(int donationProgramId)
        {
            var donation = _context.DonationPrograms.FirstOrDefault(n => n.DonationProgramId == donationProgramId);
            if (donation != null)
            {
                _context.DonationPrograms.Remove(donation);
                return true;
            }

            return false;
        }
        public void Donate(DonationDto donationDto, int UserId)
        {
            Donation donation = new Donation();
            
            donation.UserId = UserId;
            

            int donationProgramId = donationDto.donationProgram.DonationProgramId;
            DonationProgram donationProgram = _context.DonationPrograms.FirstOrDefault(n => n.DonationProgramId == donationProgramId);

            int NewTotal = donationProgram.Total + donationDto.amount;
            
            donationProgram.Total = NewTotal;
            _context.SaveChanges();

            //var donations = _context.DonationProgramAudits.ToList();

            var donationProgramAudit =
                _context.DonationProgramAudits.Where(n =>
                    n.DonationProgramId == donationProgramId).ToList().Last();

            donation.DonationProgram_AuditId = donationProgramAudit.DonationProgram_AuditId;
            donation.Amount = donationDto.amount;

            _context.Donations.Add(donation);
        }

        #endregion

        #region Users Funstions
        public User GetUserDetails(int userId)
        {
            var user = _context.Users.FirstOrDefault(n => n.UserId == userId);
            user.Password = null;
            return user;
        }

        public bool UserNameExists(string Username)
        {
            return _context.Users.Any(n => n.Username == Username);
        }

        public bool EmailExists(string Email)
        {
            return _context.Users.Any(n => n.Email == Email);
        }

        public bool DistrictNumberExists(int DistrictNumber)
        {
            return _context.DistrictNos.Any(n => n.DistrictNumber == DistrictNumber);
        }

        public User AddUser(User user)
        {
            return _context.Users.Add(user);
        }

        #endregion

        #region Order Functions

        #region GettingOrdersMethods

        public void PostNewOrder(OrderDto orderDto, int userId)
        {

            string userEmail = _context.Users.FirstOrDefault(n => n.UserId == userId).Email;

            #region queries
            string connectionString = ConfigurationManager.ConnectionStrings["RotaractDatabase"].ConnectionString;
            string ProductQuery = "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;" +
                                  " BEGIN TRANSACTION;" +
                                  " SELECT * " +
                                  " FROM   dbo.Products WITH(XLOCK); ";
            string CommitQuery = " COMMIT TRANSACTION;";


            #endregion

            List<int> productAuditIds = new List<int>();
            List<Product> products = new List<Product>();

            //The Entire SQL connection - Done this way to avoid purchasing the same object twice
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command1 = new SqlCommand(ProductQuery, connection);
                SqlDataReader reader1 = command1.ExecuteReader();
                try
                {
                    while (reader1.Read())
                    {
                        Product product = new Product();

                        product.ProductId = int.Parse(reader1["ProductId"].ToString());
                        product.Name = reader1["Name"].ToString();
                        product.Description = reader1["Description"].ToString();
                        product.ImageRef = reader1["ImageRef"].ToString();
                        product.Price = int.Parse(reader1["Price"].ToString());
                        product.Inventory = int.Parse(reader1["Inventory"].ToString());

                        products.Add(product);
                    }
                }
                finally
                {
                    reader1.Close();
                }

                if (CanBeConfirmed(orderDto.OrderItems, products))
                {
                    for (int i = 0; i < orderDto.OrderItems.Count; i++)
                    {
                        int productId = orderDto.OrderItems[i].product.ProductId;
                        for (int j = 0; j < products.Count; j++)
                        {
                            if (products[j].ProductId == productId)
                            {
                                int NewInventory = products[j].Inventory - orderDto.OrderItems[i].Quantity;
                                string UpdateInventoryQuery = " UPDATE dbo.Products" +
                                                              " SET dbo.Products.Inventory = " + NewInventory +
                                                              " WHERE Productid = " + productId;
                                SqlCommand command2 = new SqlCommand(UpdateInventoryQuery, connection);
                                SqlDataReader reader2 = command2.ExecuteReader();
                                try { while (reader2.Read()) { } }
                                finally { reader2.Close(); }
                            }
                        }
                    }


                    SqlCommand command3 = new SqlCommand(CommitQuery, connection);
                    SqlDataReader reader3 = command3.ExecuteReader();
                    try { while (reader3.Read()) { } }
                    finally { reader3.Close(); }
                    connection.Close();


                    productAuditIds = UpdateProductInventories(orderDto.OrderItems);

                    int NewOrderId = PlaceNewOrder(userId, orderDto.TotalPrice, "CONFIRMED");

                    PlaceOrderItems(productAuditIds, orderDto.OrderItems, NewOrderId);
                    PlaceShippingDetails(orderDto.ShippingDetailsModel, NewOrderId);

                    Thread t = new Thread(() =>
                        EmailController.SendEmailAboutStatusOfOrder(NewOrderId, orderDto.ShippingDetailsModel.City, userEmail, "CONFIRMED", true));
                    t.Start();
                }
                else
                {

                    SqlCommand command3 = new SqlCommand(CommitQuery, connection);

                    SqlDataReader reader3 = command3.ExecuteReader();

                    try { while (reader3.Read()) { } }
                    finally { reader3.Close(); }

                    connection.Close();

                    productAuditIds = DontUpdateProductInventories(orderDto.OrderItems);

                    int NewOrderId = PlaceNewOrder(userId, orderDto.TotalPrice, "PENDING");
                    PlaceOrderItems(productAuditIds, orderDto.OrderItems, NewOrderId);
                    PlaceShippingDetails(orderDto.ShippingDetailsModel, NewOrderId);

                    Thread t = new Thread(() =>
                        EmailController.SendEmailAboutStatusOfOrder(NewOrderId, orderDto.ShippingDetailsModel.City, userEmail, "PENDING", true));
                    t.Start();
                }
            }
        }

        public List<OrderDto> GetAllOrders(string status)
        {
            List<OrderDto> orderDtos = new List<OrderDto>();

            List<Order> orders = _context.Orders.Where(n => n.Status == status).ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                OrderDto order = new OrderDto();
                order.Status = orders[i].Status;
                order.UserId = orders[i].UserId;
                order.OrderId = orders[i].OrderId;
                order.DateStamp = orders[i].DateStamp.ToString("yyyy-MM-dd");
                order.TotalPrice = orders[i].TotalPrice;
                order.ShippingDetailsModel = GetShippingDetailsDto(orders[i].OrderId);
                order.OrderItems = GetOrderItemsDto(status,orders[i].OrderId);

                orderDtos.Add(order);
            }

            return orderDtos;
        }

        public PagedList<OrderDto> GetAllOrdersByStatus(string status,PageResourseParameters pageResourseParameters)
        {
            if (pageResourseParameters == null)
            {
                throw new ArgumentNullException(nameof(pageResourseParameters));
            }


            List<OrderDto> orderDtos = new List<OrderDto>();

            List<Order> orders = _context.Orders.Where(n => n.Status == status).OrderByDescending(p=>p.DateStamp).ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                OrderDto order = new OrderDto();
                order.Status = orders[i].Status;
                order.UserId = orders[i].UserId;
                order.OrderId = orders[i].OrderId;
                order.DateStamp = orders[i].DateStamp.ToString("yyyy-MM-dd");
                order.TotalPrice = orders[i].TotalPrice;
                order.ShippingDetailsModel = GetShippingDetailsDto(orders[i].OrderId);
                order.OrderItems = GetOrderItemsDto(status,orders[i].OrderId);

                orderDtos.Add(order);
            }
            

            return PagedList<OrderDto>.Create(orderDtos,
                pageResourseParameters.PageNumber,
                pageResourseParameters.PageSize);
        }

        public List<OrderDto> GetAllOrdersById(int UserId)
        {
            List<OrderDto> orderDtos = new List<OrderDto>();

            List<Order> orders = _context.Orders.Where(n => n.UserId == UserId).ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                OrderDto order = new OrderDto();
                order.UserId = UserId;
                order.OrderId = orders[i].OrderId;
                order.Status = orders[i].Status;
                order.DateStamp = orders[i].DateStamp.ToString("yyyy-MM-dd");
                order.TotalPrice = orders[i].TotalPrice;
                order.ShippingDetailsModel = GetShippingDetailsDto(orders[i].OrderId);
                order.OrderItems = GetOrderItemsDto(orders[i].Status, orders[i].OrderId);

                orderDtos.Add(order);
            }

            return orderDtos;
        }

        public PagedList<OrderDto> GetAllOrdersById(int UserId, PageResourseParameters pageResourseParameters)
        {
            if (pageResourseParameters == null)
            {
                throw new ArgumentNullException(nameof(pageResourseParameters));
            }

            List<OrderDto> orderDtos = new List<OrderDto>();

            List<Order> orders = _context.Orders.Where(n => n.UserId == UserId).OrderByDescending(p => p.DateStamp).ToList();

            for (int i = 0; i < orders.Count; i++)
            {
                OrderDto order = new OrderDto();
                order.UserId = UserId;
                order.OrderId = orders[i].OrderId;
                order.Status = orders[i].Status;
                order.DateStamp = orders[i].DateStamp.ToString("yyyy-MM-dd");
                order.TotalPrice = orders[i].TotalPrice;
                order.ShippingDetailsModel = GetShippingDetailsDto(orders[i].OrderId);
                order.OrderItems = GetOrderItemsDto(orders[i].Status, orders[i].OrderId);

                orderDtos.Add(order);
            }

            return PagedList<OrderDto>.Create(orderDtos,
                pageResourseParameters.PageNumber,
                pageResourseParameters.PageSize);

        }
        public bool UpdateStatusOfOrder(OrderDto orderDto)
        {
            var OrderExists = _context.Orders.FirstOrDefault(n => n.OrderId == orderDto.OrderId);
            bool error = true;
            string emailQuery = "SELECT  dbo.Users.UserId,dbo.Users.Username,dbo.Users.Email,dbo.Users.FirstName,dbo.Users.LastName,dbo.Users.DistrictNumber,dbo.Users.Role,dbo.Users.Password" +
                                " FROM dbo.Users" +
                                " JOIN dbo.Orders" +
                                     " ON dbo.Orders.UserId = dbo.Users.UserId" +
                                " WHERE dbo.Orders.OrderId = " + orderDto.OrderId + "";

            User user = _context.Users.SqlQuery(emailQuery).ToList().First();

            switch (OrderExists.Status)
            {
                case "CANCELLED":
                    break;
                case "PENDING":
                    switch (orderDto.Status)
                    {
                        case "CANCELLED":
                            OrderExists.Status = orderDto.Status;
                            error = false;
                            break;
                        case "PENDING":
                            break;
                        case "CONFIRMED":
                             break;
                        case "INPROGRESS":
                            break;
                        case "SHIPPING":
                            break;
                        case "COMPLETED":
                            break;
                    }
                    break;
                case "CONFIRMED":
                    switch (orderDto.Status)
                    {
                        case "CANCELLED":
                            Thread updateProducThread = new Thread(() => MakeProductsAvailable(orderDto.OrderId));
                            updateProducThread.Start();
                            OrderExists.Status = "CANCELLED";
                            error = false;
                            break;
                        case "PENDING":
                            break;
                        case "CONFIRMED":
                            break;
                        case "INPROGRESS":
                            OrderExists.Status = orderDto.Status;
                            error = false;
                            break;
                        case "SHIPPING":
                            break;
                        case "COMPLETED":
                            break;
                    }
                    break;
                case "INPROGRESS":
                    switch (orderDto.Status)
                    {
                        case "CANCELLED":
                            Thread updateProducThread = new Thread(() => MakeProductsAvailable(orderDto.OrderId));
                            updateProducThread.Start();
                            OrderExists.Status = orderDto.Status;
                            error = false;
                            break;
                        case "PENDING":
                            break;
                        case "CONFIRMED":
                            break;
                        case "INPROGRESS":
                            break;
                        case "SHIPPING":
                            OrderExists.Status = orderDto.Status;
                            error = false;
                            break;
                        case "COMPLETED":
                            break;
                    }
                    break;
                case "SHIPPING":
                    switch (orderDto.Status)
                    {
                        case "CANCELLED":
                            Thread updateProducThread = new Thread(() => MakeProductsAvailable(orderDto.OrderId));
                            updateProducThread.Start();
                            OrderExists.Status = orderDto.Status;
                            error = false;
                            break;
                        case "PENDING":
                            break;
                        case "CONFIRMED":
                            break;
                        case "INPROGRESS":
                           break;
                        case "SHIPPING":
                            break;
                        case "COMPLETED":
                            OrderExists.Status = orderDto.Status;
                            error = false;
                            break;
                    }
                    break;
                case "COMPLETED": 
                    break;
            }

            if (error)
            {
                return false;
            }

            //Send Email thread 
            Thread emailThread = new Thread(() => EmailController.SendEmailAboutStatusOfOrder(OrderExists.OrderId, "", user.Email, orderDto.Status, false));
            emailThread.Start();

            return true;
        }

        private ShippingDetailsDto GetShippingDetailsDto(int orderId)
        {
            ShippingDetailsDto shippingDetailsDto = new ShippingDetailsDto();

            ShippingDetails shippingDetails = _context.ShippingDetailses.FirstOrDefault(n => n.OrderId == orderId);

            shippingDetailsDto.Country = shippingDetails.Country;
            shippingDetailsDto.City = shippingDetails.City;
            shippingDetailsDto.AddressLine = shippingDetails.AddressLine;
            shippingDetailsDto.CountyOrRegion = shippingDetails.CountyOrRegion;
            shippingDetailsDto.PostalCode = shippingDetails.PostalCode;
            shippingDetailsDto.FirstName = shippingDetails.FirstName;
            shippingDetailsDto.LastName = shippingDetails.LastName;
            shippingDetailsDto.PhoneNumber = shippingDetails.PhoneNumber;

            return shippingDetailsDto;
        }
        private List<OrderItemDto> GetOrderItemsDto(string status,int orderId)
        {
            List<OrderItemDto> orderItemDto = new List<OrderItemDto>();

            List<OrderItem> orderItems = _context.OrderItems.Where(n => n.OrderId == orderId).ToList();

            for (int i = 0; i < orderItems.Count; i++)
            {
                OrderItemDto orderItem = new OrderItemDto();
                Product product = new Product();
                orderItem.product = product;

                int productAuditId = orderItems[i].Product_AuditId;
                Products_audit productAudit = new Products_audit();
                if (status == "PENDING")
                {
                    string ProductAuditQuery = "SELECT  o.Product_AuditId,o.ProductId,o.Name,o.Description,o.ImageRef,o.Price, p.Inventory, o.Updated_at,o.Operation" +
                                               "  FROM dbo.Products_audit o" +
                                               "  JOIN dbo.Products p" +
                                               "     ON p.Productid = o.ProductId" +
                                               "  WHERE o.Product_AuditId = " + productAuditId; 
                    productAudit = _context.ProductsAudits.SqlQuery(ProductAuditQuery).ToList().First();
                }
                else
                {
                    productAudit = _context.ProductsAudits.FirstOrDefault(n=>n.Product_AuditId == productAuditId);
                }

                orderItem.product.ProductId = productAudit.ProductId;
                orderItem.product.Description = productAudit.Description;
                orderItem.product.ImageRef = null; 
                orderItem.product.Name = productAudit.Name;
                orderItem.product.ProductId = productAudit.ProductId;
                orderItem.product.Price = productAudit.Price;
                orderItem.product.Inventory = productAudit.Inventory;

                orderItem.Quantity = orderItems[i].Quantity;
                orderItemDto.Add(orderItem);
            }
            return orderItemDto;
        }

        private bool CanBeConfirmed(List<OrderItemDto> orderItems, List<Product> products)
        {
            foreach (OrderItemDto orderItemDto in orderItems)
            {
                int productId = orderItemDto.product.ProductId;
                int capacity = 0;
                foreach (Product product in products)
                {
                    if (product.ProductId == productId)
                    {
                        capacity = product.Inventory;
                    }
                }
                if (orderItemDto.Quantity > capacity) { return false; }
            }
            return true;
        }
        private bool CanBeConfirmedDto(List<OrderItemDto> orderItemDtos)
        {
            foreach (OrderItemDto orderItemDto in orderItemDtos)
            { 
                int productId = orderItemDto.product.ProductId;
                int capacity = _context.Products.FirstOrDefault(n => n.ProductId == productId).Inventory;

                if (orderItemDto.Quantity > capacity)
                {
                    return false;
                }
            }
            return true;
        }

        private int PlaceNewOrder(int userId, double totalPrice, string Status)
        {
            Order newOrder = new Order();
            newOrder.UserId = userId;
            newOrder.Status = Status;
            newOrder.DateStamp = DateTime.Now;
            newOrder.TotalPrice = totalPrice;

            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            Order OrderExists = _context.Orders.Where(p => p.UserId == newOrder.UserId).ToList().Last();

            return OrderExists.OrderId;
        }

        private void PlaceOrderItems(List<int> AuditIds, List<OrderItemDto> orderItemModels, int orderId)
        {
            for (int i = 0; i < AuditIds.Count; i++)
            {
                OrderItem productItem = new OrderItem();
                productItem.OrderId = orderId;
                productItem.Product_AuditId = AuditIds[i];
                productItem.Quantity = orderItemModels[i].Quantity;

                _context.OrderItems.Add(productItem);
                _context.SaveChanges();
            }
        }

        private void PlaceShippingDetails(ShippingDetailsDto shippingDetailModels, int orderId)
        {
            ShippingDetails shippingDetails = new ShippingDetails();
            shippingDetails.OrderId = orderId;
            shippingDetails.Country = shippingDetailModels.Country;
            shippingDetails.City = shippingDetailModels.City;
            shippingDetails.AddressLine = shippingDetailModels.AddressLine;
            shippingDetails.CountyOrRegion = shippingDetailModels.CountyOrRegion;
            shippingDetails.PostalCode = shippingDetailModels.PostalCode;
            shippingDetails.FirstName = shippingDetailModels.FirstName;
            shippingDetails.LastName = shippingDetailModels.LastName;
            shippingDetails.PhoneNumber = shippingDetailModels.PhoneNumber;

            _context.ShippingDetailses.Add(shippingDetails);
            _context.SaveChanges();
        }

        #endregion

        #region UpdatingPendingOrders
        public void CheckPendingOrders()
        {
            List<Order> pendingOrders = _context.Orders.Where(n => n.Status == "PENDING").ToList();

            for (int i = 0; i < pendingOrders.Count; i++)
            {
                int orderId = pendingOrders[i].OrderId;

                List<OrderItem> pendingOrderItemsOfOrder = _context.OrderItems.Where(n => n.OrderId == orderId).ToList();
                List<OrderItemDto> pendingOrderItemsOfOrderItemDtos = new List<OrderItemDto>();

                for (int j = 0; j < pendingOrderItemsOfOrder.Count; j++)
                {
                    int productAuditId = pendingOrderItemsOfOrder[j].Product_AuditId;
                    int productid = _context.ProductsAudits.FirstOrDefault(n => n.Product_AuditId == productAuditId)
                        .ProductId;
                    OrderItemDto orderItemDto = new OrderItemDto();
                    Product product = _context.Products.FirstOrDefault(n => n.ProductId == productid);

                    orderItemDto.product = product;
                    orderItemDto.Quantity = pendingOrderItemsOfOrder[j].Quantity;

                    pendingOrderItemsOfOrderItemDtos.Add(orderItemDto);
                }

                if (CanBeConfirmedDto(pendingOrderItemsOfOrderItemDtos))
                {
                    for (int j = 0; j < pendingOrderItemsOfOrderItemDtos.Count; j++)
                    {
                        int productId = pendingOrderItemsOfOrderItemDtos[j].product.ProductId;
                        Product product = _context.Products.FirstOrDefault(n => n.ProductId == productId);
                        int NewInventory = product.Inventory - pendingOrderItemsOfOrderItemDtos[j].Quantity;

                        product.Inventory = NewInventory;
                        pendingOrders[i].Status = "CONFIRMED";
                        _context.SaveChanges();

                        int userId = pendingOrders[i].UserId;
                        string userEmail = _context.Users.FirstOrDefault(n => n.UserId == userId).Email;

                        Thread t = new Thread(() =>
                            EmailController.SendEmailAboutStatusOfOrder(pendingOrders[i].OrderId, "", userEmail,
                                "CONFIRMED", false));
                        t.Start();
                    }
                }
            }
        }

        #endregion

        #region Realeasing/Engaging Inverntory
        public void MakeProductsAvailable(int OrderId)
        {
            List<OrderItem> items = _context.OrderItems.Where(n => n.OrderId == OrderId).ToList();

            for (int i = 0; i < items.Count; i++)
            {
                int productAuditId = items[i].Product_AuditId;
                Products_audit productAudit =_context.ProductsAudits.FirstOrDefault(n => n.Product_AuditId == productAuditId);

                Product product = new Product();

                product.ProductId = productAudit.ProductId;
                product.Inventory = items[i].Quantity;
                
                AddInventory(product);
            }
        }
        private List<int> UpdateProductInventories(List<OrderItemDto> orderItemDtos)
        {
            List<int> results = new List<int>();

            foreach (var orderItemDto in orderItemDtos)
            {
                int productId = orderItemDto.product.ProductId;

                Products_audit productsAudit =
                    _context.ProductsAudits.Where(n => n.ProductId == productId).ToList().Last();
                int result = productsAudit.Product_AuditId; 
                results.Add(result);
            }
            return results;
        }
        private List<int> DontUpdateProductInventories(List<OrderItemDto> orderItemDtos)
        {
            List<int> results = new List<int>();
            foreach (var orderItemDto in orderItemDtos)
            {
                int productId = orderItemDto.product.ProductId;
                int productAuditID = _context.ProductsAudits.Where(n => n.ProductId == productId).ToList().Last().Product_AuditId;

                results.Add(productAuditID);
            }
            return results;
        }
        #endregion

        #endregion

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}