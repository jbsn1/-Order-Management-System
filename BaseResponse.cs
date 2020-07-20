using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestOrderProject.Response
{
    public class BaseResponse
    {
        public Object Data { get; set; }
        public Controllers.OrderManagementController.Status statusCode { get; set; }

        public string status { get; set; }
        public string statusMessage { get; set; }
    }
}