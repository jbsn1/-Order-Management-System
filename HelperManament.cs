using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestOrderProject.BusinessLogic;
using TestOrderProject.Interfaces;
using TestOrderProject.Models;

namespace TestOrderProject.Helper
{
    public class HelperManament : IOrderManagement
    {
        public  string CreateOrder(OrderrequestDetails orderrequestDetails)
        {
            return new BusinessLogicDao().CreateOder(orderrequestDetails);
        }
        public List<Order> GetOrders()
        {
            return new BusinessLogicDao().GetOrders();
        }
        public int DeleteOrder(string OrderNO)
        {
            return new BusinessLogicDao().DeleteOrder(OrderNO);
        }
        public UserDetails GetUserByCode(string usercode, string password)
        {
            return new BusinessLogicDao().GetUserByCode(usercode, password);
        }
        public List<Order> GetOrderByBuyerLogin(string usercode)
        {
            return new BusinessLogicDao().GetOrderByBuyerLogin(usercode);
        }
        public List<Order> GetOrderDetailsByOrderNumber(string OrderNO)
        {
            return new BusinessLogicDao().GetOrderDetailsByOrderNumber(OrderNO);
        }
        public bool UpdateOrderStatus(string Orderno, string Status)
        {
            return new BusinessLogicDao().UpdateOrderStatus(Orderno, Status);
        }
        
        public List<Order> GetOrdersByuser(string UserCode)
        {
            return new BusinessLogicDao().GetOrdersByuser(UserCode);
        }
        
    }
}