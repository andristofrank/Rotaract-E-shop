using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RotaractServer.Helpers;
using RotaractServer.Models;
using RotaractServer.ResourseParameters;

namespace RotaractServer.Services
{
    public interface IRotaractRepository
    {
        #region Products Functions
        IEnumerable<Product> GetProducts();
        Product GetProduct(int productId);
        PagedList<Product> GetProducts(PageResourseParameters pageResourseParameters);
        void AddProduct(Product product);
        bool UpdateProduct(Product product);
        bool AddInventory(Product product);
        bool DeleteProduct(int productId);

        #endregion

        #region Donations Functions

        PagedList<DonationProgram> GetActiveDonationPrograms(PageResourseParameters pageResourseParameters);
        List<DonationProgram> GetDonationPrograms();
        PagedList<DonationProgram> GetDonationPrograms(PageResourseParameters pageResourseParameters);
        List<DonationProgram> GetActiveDonationPrograms();
        PagedList<DonationProgram> GetDonationProgramsById(int UserId,PageResourseParameters pageResourseParameters);
        void AddDonationProgram(DonationProgram donationProgram);
        void UpdateDonation(DonationProgram donationProgram);
        bool DeleteDonationProgram(int donationProgramId);
        void Donate(DonationDto donationDto, int UserId);
        #endregion

        #region Users Funstions
        User GetUserDetails(int userId);
        bool UserNameExists(string Username);
        bool EmailExists(string Email);
        bool DistrictNumberExists(int DistrictNumber);
        User AddUser(User user);

        #endregion

        #region Order Functions

        void PostNewOrder(OrderDto orderDto, int userId);
        List<OrderDto> GetAllOrders(string status);
        PagedList<OrderDto> GetAllOrdersByStatus(string status, PageResourseParameters pageResourseParameters);
        List<OrderDto> GetAllOrdersById(int UserId); 
        PagedList<OrderDto> GetAllOrdersById(int UserId, PageResourseParameters pageResourseParameters);

        bool UpdateStatusOfOrder(OrderDto orderDto);
        void CheckPendingOrders();
        void MakeProductsAvailable(int OrderId);
        #endregion
        bool Save();
    }
}
