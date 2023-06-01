using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IPlacingAdsInPageActions
    {
        public List<PlacingAdsInPage> GetAllPlacingAdsInPages();

        public void AddNewPlacingAdInPage(PlacingAdsInPage placingAdInPage);

        public void UpdatePlacingAdInPage(int id, PlacingAdsInPage placingAdInPage);

        public void DeletePlacingAdInPage(int id);
    }
}
