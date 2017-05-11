using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication9.Models.exSearch
{
    public class checkBoxListModel
    {
        public checkBoxListModel(int Id, string Name, string Group)
        {
            this.Id = Id;
            this.Name = Name;
            this.Group = Group;
        }
        public int Id { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public bool ifSelected { get; set; }
    }
}