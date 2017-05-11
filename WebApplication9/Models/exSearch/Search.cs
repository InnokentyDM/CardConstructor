using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication9.DAO;
using WebApplication9.Models.exSearch;

namespace WebApplication9.Models
{
    public class Search
    {
        ApplicationDAO applicationDAO = new ApplicationDAO();

        ExtandedSearchModel exSearch = new ExtandedSearchModel();
        public List<checkBoxListModel> availItems { get; set; }
        public List<checkBoxListModel> selectedItems { get; set; }
        public exSearch.PostedItems postedItems { get; set; }
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

        List<cards> cards = new List<cards>();

        public int Id { get; set; }
        public Nullable<int> layout_id { get; set; }
        public string text { get; set; }
        public string image { get; set; }
        public string USER_ID { get; set; }
        public bool published { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<application> application { get; set; }
        public virtual AspNetUsers AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CARD_CLAIMS> CARD_CLAIMS { get; set; }
        public virtual layout layout { get; set; }

        public List<cards> Cards
        {
            get
            {
                return cards;
            }

            set
            {
                cards = value;
            }
        }

      

    }
}