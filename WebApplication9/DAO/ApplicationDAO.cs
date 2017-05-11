using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication9.Models;
using System.IO;
using System.Data.SqlClient;
using Microsoft.Ajax.Utilities;

namespace WebApplication9.DAO
{
    public class ApplicationDAO
    {
        private Entities1 _entities = new Entities1();

        //Thes method returns list of cards for extanded search
        public IEnumerable<cards> cardsForExtandedSearchList(string[] propertyList, string itemType)
        {
            List<CARD_CLAIMS> claimsQuery = new List<CARD_CLAIMS>();
            List<cards> cardsQuery = new List<cards>();
            foreach (var i in propertyList)
            {
                var claimsTmp = from c in _entities.CARD_CLAIMS where c.ClaimValue == i select c;
                claimsQuery.AddRange(claimsTmp);
            }

            int itemT = Int32.Parse(itemType);

            foreach (var i in claimsQuery)
            {
                var cardsTmp = from c in _entities.cards where c.Id == i.Card_id && c.ITEM_TYPE.Id == itemT select c;
                cardsQuery.AddRange(cardsTmp);
            }

            return cardsQuery;
        }

        //This method returns list of cards claims for extanded search
        public IEnumerable<CARD_CLAIMS> selectedItemsForExtandedSearch(string[] propertyList)
        {
            List<CARD_CLAIMS> claimsQuery = new List<CARD_CLAIMS>();
            foreach(var i in propertyList)
            {
                var claimsTmp = from c in _entities.CARD_CLAIMS where c.ClaimValue == i select c;
                var res = claimsTmp.GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());
                claimsQuery.AddRange(res);
            }
            return claimsQuery;
        }

        public IEnumerable<CARD_CLAIMS> getCardClaims(int id)
        {          
            var claimsTmp = from c in _entities.CARD_CLAIMS where c.Card_id == id select c;          
            return claimsTmp;
        }

        //This method returns cards where card claim type = id
        public IEnumerable<cards> getItemLayouts(String id)
        {
            //var claims = from c in _entities.CARD_CLAIMS where c.ClaimValue == id select c;
            //List < cards > cardsQuery = new List<cards>();
            //foreach (var i in claims)
            //{
            //    var tmp = from c in _entities.cards where c.Id == i.Card_id select c;
            //    cardsQuery.AddRange(tmp);
            //}
            int tmpId = Int32.Parse(id);
            var cards = from c in _entities.cards where c.ITEM_TYPE.Id == tmpId && c.published == true select c;
            return (cards);
        }

        public IEnumerable<ITEM_TYPE> getAllItemTypes()
        {
            var res = from c in _entities.ITEM_TYPE select c;
            return res;
        }

        public IEnumerable<CARD_CLAIMS> getColorClaims()
        {
            var claims = (from c in _entities.CARD_CLAIMS select c);
            var res = claims.Where(c => c.ClaimType == "COLOR").GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());
            return res;
        }

        public IEnumerable<CARD_CLAIMS> getStyleClaims()
        {
            var claims = (from c in _entities.CARD_CLAIMS select c);
            var res = claims.Where(c => c.ClaimType == "STYLE").GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());
            return res;
        }


        public IEnumerable<CARD_CLAIMS> getGroupClaims()
        {
            var claims = (from c in _entities.CARD_CLAIMS select c);
            var res = claims.Where(c => c.ClaimType == "GROUP").GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());
            return res;
        }

        public IEnumerable<CARD_CLAIMS> getAllCardClaims()
        {
            var claims = (from c in _entities.CARD_CLAIMS select c);
            var res = claims.GroupBy(c => c.ClaimValue).Select(g => g.FirstOrDefault());
                return res;
        }

        //public IEnumerable<COLORS> getAllColors()
        //{
        //    return (from c in _entities.COLORS select c);
        //}


        //public IEnumerable<ITEM_STYLE> getAllStyles()
        //{
        //    return (from c in _entities.ITEM_STYLE select c);
        //}


        //public IEnumerable<PROFESSION> getProfessions()
        //{
        //    return (from c in _entities.PROFESSION select c);
        //}
        //public IEnumerable<CARD_GROUP> getAllGroups()
        //{
        //    return (from c in _entities.CARD_GROUP select c);
        //}

        public IEnumerable<application> getAllApplication()
        {
            return (from c in _entities.application select c).OrderByDescending(c => c.Id);
        }
        public IEnumerable<application> getApplication(int id)
        {
            return (from c in _entities.application where c.Id == id select c).OrderByDescending(c => c.Id);
        }

     public IEnumerable<application> ManagerApplicationList()
        {
            return (from c in _entities.application where c.status == "Обработка заявки" select c).OrderByDescending(c => c.Id);
        }

     public IEnumerable<application> PrintApplicationList()
     {
         return (from c in _entities.application where c.status == "Печать" select c).OrderByDescending(c => c.Id);
     }

     public IEnumerable<application> DoneApplicationList()
     {
         return (from c in _entities.application where c.status == "Готово" select c).OrderByDescending(c => c.Id);
     }

     public void Delete(int id)
     {
         var query = from c in _entities.application where c.Id == id select c;
         foreach (application app in query)
         {
             app.status = "This application is stopped";
         }
         try
         {
             _entities.SaveChanges();
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             // Provide for exceptions.
         }
     }

     public void ApplicationIsDone(int id)
     {
         var query = from c in _entities.application where c.Id == id select c;
         foreach (application app in query)
         {
             app.status = "Готово";
         }
         try
         {
             _entities.SaveChanges();
         }
         catch (Exception e)
         {
             Console.WriteLine(e);
             // Provide for exceptions.
         }
     }


        public void updateStatus(int id)
        {
            var query = from c in _entities.application where c.Id == id select c;
            foreach (application app in query)
            {
                app.status = "Печать";
            }
            try
            {
                _entities.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
            }         
        }

        public IEnumerable<layout> getLayout(int id)
        {
            return (from c in _entities.layout where c.Id == id select c);
        }

     

        public IEnumerable<materials> getMaterial(int id)
        {
            return (from c in _entities.materials where c.Id == id select c);
        }
        public bool AddNewApplication(application application)
        {
            try
            {
                _entities.application.Add(application);
                _entities.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
                
            }
            return true;
            
        }

        public bool AddNewCard(cards cards)
        {
            try
            {   
                _entities.cards.Add(cards);
                _entities.SaveChanges();
                return true;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }


        public bool AddNewLayout(layout layout)
        {
            try
            {
                _entities.layout.Add(layout);
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;

        }

        public bool AddNewMaterial(materials materials)
        {
            try
            {
                _entities.materials.Add(materials);
                _entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;

        }





        public IEnumerable<cards> getAllCards()
        {
            return (from c in _entities.cards select c);
        }

        public IEnumerable<layout> getAllLayout()
        {
            return (from c in _entities.layout select c);
        }
      

        public IEnumerable<materials> getAllMaterials()
        {
            return (from c in _entities.materials select c);
        }


 
        public IEnumerable<application> getUserApplications(string id)
        {
            return (from c in _entities.application where c.user_id == id select c).OrderByDescending(c => c.Id);
        }

        public IEnumerable<cards> getUserCards(string id)
//=======
//        public IEnumerable<service_type> getAllServices()
//        {
//            return (from c in _entities.service_type select c);
//        }


//        public IEnumerable<application> getUserApplications(string id)
//>>>>>>> origin/master
        {
            IEnumerable<cards> tmp = (from c in _entities.cards where c.USER_ID == id select c);
            return tmp.OrderByDescending(c => c.Id);
        }

        public IEnumerable<cards> getCardById(int id)
        {           
            return (from c in _entities.cards where c.Id == id select c);
        }

        //public bool EditCardLayout(int id, string image)
        //{
        //    try
        //    {
        //        var qry = from cards in _entities.cards where cards.Id == id select cards;
        //        foreach (var item in qry)
        //        {
        //            item.image = image;   
        //        }
        //        _entities.SaveChanges();
               
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //        return false;
        //    }
        //    return true;
        //}
     
       
    }
}