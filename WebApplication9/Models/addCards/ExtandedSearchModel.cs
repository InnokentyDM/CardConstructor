using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication9.Models.addCards
{
    public class ExtandedSearchModel
    {
        public List<checkBoxListModel> availItems { get; set; }
        public List<checkBoxListModel> selectedItems { get; set; }
        public PostedItems postedItems { get; set; }
    }
}