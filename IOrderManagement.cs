using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestOrderProject.Models;

namespace TestOrderProject.Interfaces
{
    interface IOrderManagement
    {
        string CreateOrder(OrderrequestDetails orderrequestDetails);
        List<Order> GetOrders();
        int DeleteOrder(string OrderNO);
        UserDetails GetUserByCode(string usercode, string password);
        List<Order> GetOrderByBuyerLogin(string Usercode);
        List<Order> GetOrderDetailsByOrderNumber(string OrderNO);
        bool UpdateOrderStatus(string Orderno, string Status);
        List<Order> GetOrdersByuser(string UserCode);
    }
}
