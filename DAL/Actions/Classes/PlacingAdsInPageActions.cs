using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class PlacingAdsInPageActions : IPlacingAdsInPageActions
    {
        NewspaperSystemContext _dbNewspapers;

        public PlacingAdsInPageActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewPlacingAdInPage(PlacingAdsInPage placingAdInPage)
        {
            _dbNewspapers.PlacingAdsInPages.Add(placingAdInPage);
            _dbNewspapers.SaveChanges();
        }

        public void DeletePlacingAdInPage(int id)
        {
            throw new NotImplementedException();
        }

        public List<PlacingAdsInPage> GetAllPlacingAdsInPages()
        {
            return _dbNewspapers.PlacingAdsInPages.ToList();
        }

        public void UpdatePlacingAdInPage(int id, PlacingAdsInPage placingAdInPage)
        {
            var PlacingAdInPageToEdit = _dbNewspapers.PlacingAdsInPages.FirstOrDefault(x => x.PlaceInPageId == id);
            if (PlacingAdInPageToEdit != null)
            {
                PlacingAdInPageToEdit.PageId = placingAdInPage.PageId;
                PlacingAdInPageToEdit.DetailsId = placingAdInPage.DetailsId;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
