using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class LayoutViewModel
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
        public layout layout { get; set; }
    }
}