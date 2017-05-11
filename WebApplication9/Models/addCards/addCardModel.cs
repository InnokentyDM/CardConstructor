using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication9.DAO;
using WebApplication9.Models.addCards;
using System.ComponentModel.DataAnnotations;

namespace WebApplication9.Models.addCards
{
    public class addCardModel
    {
        ApplicationDAO applicationDAO = new ApplicationDAO();

        ExtandedSearchModel exSearch = new ExtandedSearchModel();
        public List<checkBoxListModel> availItems { get; set; }
        public List<checkBoxListModel> selectedItems { get; set; }
        public PostedItems postedItems { get; set; }
        public string typeId { get; set; }

        public ExtandedSearchModel getSearch()
        {
            return exSearch;
        }

        public void setSearch()
        {
            exSearch.availItems = availItems;
            exSearch.postedItems = postedItems;
            exSearch.selectedItems = selectedItems;
        }

        public cards cards { get; set; }

        public int Id { get; set; }
        public Nullable<int> layout_id { get; set; }
        [Display(Name ="Комментарий")]
        public string text { get; set; }
        public string image { get; set; }
        public string USER_ID { get; set; }
        public bool published { get; set; }
        public int ITEM_TYPE_ID { get; set; }

       

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application> application { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_CLAIMS> CARD_CLAIMS { get; set; }
        public virtual layout layout { get; set; }
        public virtual ITEM_TYPE ITEM_TYPE { get; set; }

      



    }
}