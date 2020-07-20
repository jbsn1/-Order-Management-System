using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOrderProject.Models
{
    public class OrderrequestDetails
    {
        public string ItemCodes { get; set; }
        public string UserCode { get; set; }
    }
     public class Order
    {
        public string orderno { get; set; }
        public string UserCode { get; set; }
        public string Itemcode { get; set; }
        public string  ItemName { get; set; }
        public string Status { get; set; }
        public DateTime ? OrdeDate { get; set; }
        public Decimal ? TotalPirice { get; set; }
        public Decimal? TotalTax { get; set; }
        public Decimal? TotalDiscount { get; set; }
        public List<OrderItemDetais> orderitemlist { get; set; }
    }

    public class OrderItemDetais
    {
        public string orderno { get; set; }
        public string  Itemcode { get; set; }
        public Decimal? TotalPirice { get; set; }
        public Decimal? TotalTax { get; set; }
        public Decimal? TotalDiscount { get; set; }
        public string Image { get; set; }
        public double ? Height { get; set; }
        public double ? Weight { get; set; }
        public string barcode { get; set; }
        public double? StockKeepingUnit { get; set; }
    }
    public class UserDetails
    {
        public string UserCode { get; set; }
        public decimal? mobilenumber { get; set; }
    }


}