using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestOrderProject.Models;
using TestOrderProject.Common;
using System.Web.Http;

namespace TestOrderProject.BusinessLogic
{
    public class BusinessLogicDao
    {
        //createorder and update 
        public string CreateOder(OrderrequestDetails orderrequestDetails)
        {
            string OrderCode = "";
            try
            {
                using (var db = new Entities())
                {
                    using (var dbContextTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                        GerOrderCode:
                            var Ordercode = CreateRando5DigitOrderCode();
                            if (Ordercode != "")
                            {
                                if ((db.tblOrders.Where(i => i.Orderno.Equals(Ordercode, StringComparison.CurrentCultureIgnoreCase))).Any())
                                {
                                    goto GerOrderCode;
                                }
                            }
                            else
                            {
                                goto GerOrderCode;
                            }
                            tblOrder order = new tblOrder();

                            order.Orderno = OrderCode;
                            order.UserCode = orderrequestDetails.UserCode;
                            db.tblOrders.Add(order);
                            db.SaveChanges();
                            var orderid = order.id;


                            decimal? totalamount = 0m;
                            var Orders = orderrequestDetails.ItemCodes.Split(',');
                            int line = 0;
                            if (orderid > 0)
                            {
                                foreach (var orderItemCode in Orders)
                                {
                                    OrderItem orderItem = new OrderItem();

                                    var item = db.tblItems.FirstOrDefault(i => i.ItemCode.Equals(orderItemCode, StringComparison.CurrentCultureIgnoreCase));
                                    if (item != null)
                                    {
                                        line++;
                                        orderItem.Line = line;
                                        orderItem.Orderno = Ordercode.ToString();
                                        orderItem.ItemCode = item.ItemCode;
                                        orderItem.TotalPirice = item.Price;
                                        orderItem.barcode = item.barcode;
                                        orderItem.StockKeepingUnit = item.StockKeepingUnit;
                                        orderItem.Height = item.Height;
                                        orderItem.Weight = item.Weight;
                                        orderItem.Image = item.Image;
                                        
                                        totalamount += item.Price;
                                        db.OrderItems.Add(orderItem);
                                    }
                                }
                                db.SaveChanges();
                                updatePricesInOrder(Ordercode, totalamount);
                                /*Mail sending */
                                Common.Common.SendMailNotification(OrderCode, orderrequestDetails.UserCode);
                            }
                            dbContextTransaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            Common.Common.SaveException(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Common.SaveException(ex);
            }
            return OrderCode;
        }
        //generate new  order number  everytime when we place new order
        public string CreateRando5DigitOrderCode()
        {
            string r;
            Random generator = new Random();
            return r = generator.Next(0, 999999).ToString("D6");
        }
        //updateorder itemprice
        public void updatePricesInOrder(string Ordercode, decimal? totalamount)
        {
            if (Ordercode != "")
            {
                try
                {
                    tblOrder tblItem = Common.Common.GetOrderById(Ordercode);
                    using (var db = new Entities())
                        if (tblItem != null)
                        {
                            tblItem.TotalPirice = totalamount;
                            db.SaveChanges();
                        }
                }
                catch (Exception ex)
                {
                    Common.Common.SaveException(ex);
                }
            }
        }
        //view all orders 
        public List<Order> GetOrders()
        {

            List<Order> objOrderItem = null;
            try
            {
                using (var db = new Entities())
                {



                    objOrderItem = (from O in db.tblOrders.AsQueryable()
                                    select new Order
                                    {
                                        orderno = O.Orderno,
                                        TotalPirice = O.TotalPirice,
                                        TotalTax = O.TotalTax,
                                        TotalDiscount = O.TotalDiscount,
                                        OrdeDate = O.OrdeDate,
                                        Status = O.Status,
                                        orderitemlist = db.OrderItems.Where(i => i.Orderno == O.Orderno).Select(ii => new OrderItemDetais
                                        {
                                            orderno = ii.Orderno,
                                            Itemcode = ii.ItemCode,
                                            TotalPirice = ii.TotalPirice,
                                            TotalTax = ii.TotalTax,
                                            TotalDiscount = ii.TotalDiscount,
                                            Image = ii.Image,
                                            Height = ii.Height,
                                            Weight = ii.Weight,
                                            barcode = ii.barcode,
                                            StockKeepingUnit = ii.StockKeepingUnit,


                                        }).ToList()
                                    }).ToList();




                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return objOrderItem;
        }
        //deleteorder
        public int DeleteOrder(string OrderNO)
        {
            int value = 0;

            using (var db = new Entities())
            {
                var orderitem = from o in db.OrderItems
                                where OrderNO == o.Orderno
                                select o;
                db.Entry(orderitem).State = System.Data.Entity.EntityState.Deleted;
                var order = from o in db.tblOrders
                            where OrderNO == o.Orderno
                            select o;
                db.Entry(order).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                db.SaveChanges();
                value = 1;
            }

            return value;
        }
        //GET Orders By Role
        public List<Order> GetOrdersByuser(string UserCode)
        {

            List<Order> objOrderItem = null;
            try
            {
                using (var db = new Entities())
                {


                    var user = db.tblusers.AsQueryable().FirstOrDefault(i => i.UserCode == UserCode);
                    if (user != null && user.Role != "")
                    {
                        objOrderItem = (from O in db.tblOrders.AsQueryable()
                                        join U in db.tblusers.AsQueryable() on O.UserCode equals U.UserCode
                                        where (user.Role == "Adminstrator" || (O.UserCode == UserCode))
                                        select new Order
                                        {
                                            orderno = O.Orderno,
                                            TotalPirice = O.TotalPirice,
                                            TotalTax = O.TotalTax,
                                            TotalDiscount = O.TotalDiscount,
                                            OrdeDate = O.OrdeDate,
                                            orderitemlist = db.OrderItems.Where(i => i.Orderno == O.Orderno).Select(ii => new OrderItemDetais
                                            {
                                                orderno = ii.Orderno,
                                                Itemcode = ii.ItemCode
                                            }).ToList()
                                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return objOrderItem;
        }
        //getuserdetails by usercode
        public UserDetails GetUserByCode(string usercode, string password)
        {
            UserDetails objuser = null;
            try
            {
                using (var db = new Entities())
                {

                    objuser = (from U in db.tblusers.AsQueryable()
                               where U.UserCode.Equals(usercode, StringComparison.CurrentCultureIgnoreCase)
                             && U.Password.Equals(password, StringComparison.CurrentCultureIgnoreCase)

                               select new UserDetails
                               {
                                   UserCode = U.UserCode,
                                   mobilenumber = U.PhoneNumber

                               }).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {


            }
            return objuser;
        }

        public List<Order> GetOrderByBuyerLogin(string usercode)
        {
            List<Order> objOrder = null;
            try
            {
                using (var db = new Entities())
                {
                    objOrder = (from o in db.tblOrders
                                where o.UserCode.Equals(usercode, StringComparison.CurrentCultureIgnoreCase)
                                select new Order
                                {
                                    OrdeDate = o.OrdeDate,
                                    orderno = o.Orderno,
                                    UserCode = o.UserCode

                                }).ToList();
                }
            }
            catch (Exception ex)
            {


            }
            return objOrder;
        }
        //get orderdetails by ordernumber
        public List<Order> GetOrderDetailsByOrderNumber(string OrderNO)
        {

            List<Order> objOrderItem = null;
            try
            {
                using (var db = new Entities())
                {
                    objOrderItem = (from o in db.tblOrders
                                    join oi in db.OrderItems
                                    on o.Orderno equals oi.Orderno
                                    join i in db.tblItems
                                    on oi.ItemCode equals i.ItemCode
                                    where o.Orderno.Equals(OrderNO, StringComparison.CurrentCultureIgnoreCase)
                                    select new Order
                                    {
                                        OrdeDate = o.OrdeDate,
                                        orderno = o.Orderno,
                                        Itemcode = oi.ItemCode,
                                        ItemName = i.ItemName,
                                        TotalPirice = o.TotalPirice,
                                        TotalDiscount = o.TotalDiscount,
                                        TotalTax = o.TotalTax

                                    }).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return objOrderItem;
        }
        //updateorder status  pending , cancelled , rejected , completed 
        public bool UpdateOrderStatus(string Orderno, string Status)
        {
            bool RetStaus = false;
            tblOrder objOrders = null;
            try
            {

                using (var db = new Entities())
                {

                    objOrders = db.tblOrders.FirstOrDefault(i => i.Orderno == Orderno);


                    if (objOrders != null)
                    {
                        objOrders.Status = Status;
                        db.SaveChanges();
                        RetStaus = true;
                    }
                }

            }
            catch (Exception)
            {
                RetStaus = false;

                throw;
            }
            return RetStaus;
        }

        
      
        
    }


}