using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.Models.addCards
{
    public class checkBoxListModel
    {
        public checkBoxListModel(string Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool ifSelected { get; set; }
    }
}