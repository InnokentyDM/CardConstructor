using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication9.Models;
using WebApplication9.DAO;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9.Models
{
    public class ApplyModel
    {
        ApplicationDAO applicationDAO = new ApplicationDAO();
        public application app { get; set; }
        public Nullable<int> vc_id { get; set; }
        public cards card { get; set; }
        
        public string user_id { get; set; }
        public Nullable<System.DateTime> insertionDate { get; set; }
        public Nullable<System.DateTime> finishDate { get; set; }
        public string status { get; set; }
        public Nullable<int> count { get; set; }
        public Nullable<decimal> summ { get; set; }
        public Nullable<int> material_id { get; set; }
        public Nullable<int> layout_id { get; set; }
        public string text { get; set; }
        public string image { get; set; }
       
    }
}