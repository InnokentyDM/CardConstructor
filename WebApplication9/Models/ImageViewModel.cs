using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication9.Models
{
    public class ImageViewModel
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ImageUpload { get; set; }
        public int type_name_id { get; set; }
        public int item_layout_id { get; set; }

        public int item_type_id { get; set; }
        public virtual ELEMENTS_TYPES ELEMENTS_TYPES { get; set; }
    }
}