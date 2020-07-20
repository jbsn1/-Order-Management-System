using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using TestOrderProject.Models;
using TestOrderProject.Helper;
using TestOrderProject.Response;

namespace TestOrderProject.Controllers
{
    public class OrderManagementController : ApiController
    {
        //create  and update the order 
        [HttpPost]
        public BaseResponse CreateOder(HttpRequestMessage request, OrderrequestDetails orderrequestDetails)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;
            string OrderCode = "";
            BaseResponse response = new BaseResponse();
            try
            {
                if (orderrequestDetails != null)
                {
                    OrderCode = new HelperManament().CreateOrder(orderrequestDetails);
                }
                if (OrderCode != "")
                {
                    status = "Success";
                    statusMessage = "Create order successfully.";
                    Status = Status.Success;

                }
                else
                {
                    status = "Failure";
                    statusMessage = "Unable to Create order , Please try again.";
                    Status = Status.Failure;
                }
            }
            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again."; ;
                Status = Status.Error;
            }
            response.Data = OrderCode;
            response.status = status;
            response.statusMessage = statusMessage;
            response.statusCode = Status;

            return response;
        }
        public enum Status
        {
            Success = 200,
            Error = 0,
            Failure = 400
        }
        //view all orders 

        [HttpGet]
        public BaseResponse GetOrders(HttpRequestMessage request)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;
            bool IsValid = true;

            BaseResponse response = new BaseResponse();
            List<Order> OrderItem = null;

            try
            {
                OrderItem = new HelperManament().GetOrders();

                if (OrderItem != null)
                    {

                    status = "Success";
                    statusMessage = "Get All Order successfully.";
                    Status = Status.Success;
                   }
                    else
                    {
                    status = "Success";
                    statusMessage = "No Orders Available.";
                    Status = Status.Failure;
                }
                
            }
            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again."; ;
                Status = Status.Error;
            }

            response.Data = OrderItem;
            response.status = status;
            response.statusMessage = statusMessage;
            response.statusCode = Status;

            return response;
        }
        //delete orders 
        [HttpPost]
        public BaseResponse DeleteOrder(HttpRequestMessage request, string OrderNO)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;
            bool IsValid = true;
            bool DeleteStatus = false;
            BaseResponse response = new BaseResponse();
            try
            {
                

                    int Result = new HelperManament().DeleteOrder(OrderNO);

                    if (Result > 0)
                    {
                        DeleteStatus = true;
                        status = "Success";
                        statusMessage = "Order Deleted successfully.";
                        Status = Status.Success;

                    }
                    else
                    {
                        DeleteStatus = false;
                        status = "Failure";
                        statusMessage = "Unable to Delete the Order , Please try again.";
                        Status = Status.Failure;
                    }
                
            }
            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again."; ;
                Status = Status.Error;
            }
            response.Data = DeleteStatus;
            response.status = status;
            response.statusMessage = statusMessage;
            response.statusCode = Status;

            return response;
        }

        [HttpGet]
        public BaseResponse GetUserByCode(HttpRequestMessage request, string usercode, string password)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;
           
            BaseResponse response = new BaseResponse();
            UserDetails objLoginModel = null;
            List<Order> OrderItem = null;
            try
            {


                objLoginModel = (new HelperManament()).GetUserByCode(usercode, password);
                 if(objLoginModel!=null)
                 {
                    if(objLoginModel.UserCode=="Buyer")
                    {
                        var Usercode = objLoginModel.UserCode;
                        OrderItem = (new HelperManament()).GetOrderByBuyerLogin(Usercode);
                        
                    }
                    else
                    {
                        OrderItem = new HelperManament().GetOrders();
                    }
                    status = "Success";
                    statusMessage = "fetech orders successfully.";
                    Status = Status.Success;
                }
                else
                {
                    status = "Success";
                    statusMessage = "no orders .";
                    Status = Status.Success;
                }                   
            }

            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again."; ;
                Status = Status.Error;
            }
            response.Data = OrderItem;
            response.status = status;
            response.statusMessage = statusMessage;
            response.statusCode = Status;

            return response;
        }
        //getorderdetailsbyordernumber 
        [HttpGet]
        public BaseResponse GetOrderDetailsByOrderNumber(HttpRequestMessage request, string OrderNO)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;

            BaseResponse response = new BaseResponse();
            UserDetails objLoginModel = null;
            List<Order> OrderItem = null;
            try
            {


                OrderItem = (new HelperManament()).GetOrderDetailsByOrderNumber(OrderNO );
                if (objLoginModel != null)
                {
                  
                    status = "Success";
                    statusMessage = "fetech orders successfully.";
                    Status = Status.Success;
                }
                else
                {
                    status = "Success";
                    statusMessage = "no orders .";
                    Status = Status.Success;
                }
            }

            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again."; ;
                Status = Status.Error;
            }
            response.Data = OrderItem;
            response.status = status;
            response.statusMessage = statusMessage;
            response.statusCode = Status;

            return response;
        }
        //update  order status placed , cancelled , rejected , pending , approved , completed 
        [HttpGet]
        public BaseResponse UpdateOrderStatus(string Orderno, string StatusCode)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;
            bool result = false;
            BaseResponse response = new BaseResponse();
            try
            {
                result = (new HelperManament()).UpdateOrderStatus(Orderno, StatusCode);
                if (result)
                {
                    status = "sucess";
                    statusMessage = "Order Status  Updated succesfully";
                    Status = Status.Failure;

                }
                else
                {

                    status = "Failure";
                    statusMessage = "Order StatusNot   Updated succesfully";
                    Status = Status.Failure;
                }
            }
            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again.";
                Status = Status.Error;
            }

            response.status = StatusCode;
            response.statusMessage = statusMessage;
            response.statusCode = Status;
            response.Data = result;

            return response;
        }

        //fetch order by userrole ( adminstration , buyer )
        [HttpGet]
        public BaseResponse GetOrdersByuser(HttpRequestMessage request, string UserCode)
        {
            string status = string.Empty, statusMessage = string.Empty;
            Status Status = Status.Success;
            List<Order> result = null; 
            BaseResponse response = new BaseResponse();
            try
            {
                result = (new HelperManament()).GetOrdersByuser(UserCode);
                if (result!=null)
                {
                    status = "sucess";
                    statusMessage = "Orders Fetched succesfilly";
                    Status = Status.Failure;

                }
                else
                {

                    status = "Failure";
                    statusMessage = "Orders Not Found succesfilly";
                    Status = Status.Failure;
                }
            }
            catch (Exception ex)
            {
                status = "Failure";
                statusMessage = "Oops something went wrong. Please try again.";
                Status = Status.Error;
            }

            response.status = status;
            response.statusMessage = statusMessage;
            response.statusCode = Status;
            response.Data = result;

            return response;
        }




    }
}