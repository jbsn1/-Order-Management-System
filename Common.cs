using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOrderProject.Common
{
    public class Common
    {
        public static void SaveException(Exception ex)
        {
            using (System.IO.StreamWriter file =
           new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt", true))
            {
                file.WriteLine(ex.Message + "/n" + ex.InnerException.ToString());
            };
        }
        public static tblOrder GetOrderById(string  OrderCode)
        {
            tblOrder tblOrder = null;
            using (var db = new Entities())
                if (OrderCode != "")
                {
                    tblOrder = db.tblOrders.FirstOrDefault(i => i.Orderno.Equals(OrderCode,StringComparison.CurrentCultureIgnoreCase));
                }

            return tblOrder;
        }

        public static bool SendMailNotification(string OrderCode, string UserCode)
        {
            bool retuenValue = false;
            /*Oftenly We Write a mail service Separately by inserting  all data into one table as if status 
             * of Mailnotification table isPending then we send mail*/
            try
            {
                using (var db = new Entities())
                {
                    MailNotification mailNotification = new MailNotification();
                    mailNotification.UserCode = UserCode;
                    mailNotification.Status = "Pending";
                    mailNotification.OrderCode = OrderCode;
                    mailNotification.MailDate = DateTime.UtcNow; ;
                    mailNotification.Message = GetmessageAsperStatus(OrderCode);

                    retuenValue = true;
                    db.MailNotifications.Add(mailNotification);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                SaveException(ex);

            }
           return retuenValue;
        }
       public static string GetmessageAsperStatus( string OrderCode)
        {
            var ReturnString = "";
            using (var db=new Entities())
            {
                var Orders = db.tblOrders.FirstOrDefault(i => i.Orderno == OrderCode);
                if (Orders != null && Orders.Status != "")
                {
                    switch (Orders.Status)
                    {
                        case "Pending":
                            ReturnString = "Order Got Pending";
                                break;
                        case "Accepted":
                            ReturnString = "Order Got Accepted";
                            break;
                        case "placed":
                            ReturnString = "Order Got placed";
                            break;
                        case "Delivery":
                            ReturnString = "Order Got Delivery";
                            break;
                        case "Cancel":
                            ReturnString = "Order Got Canceled";
                            break;
                    }
                }
            }
            return ReturnString;
        }
    }
}