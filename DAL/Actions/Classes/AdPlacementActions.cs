using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class AdPlacementActions : IAdPlacementActions
    {
        NewspaperSystemContext _dbNewspapers;

        public AdPlacementActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewAdPlacement(AdPlacement adPlacement)
        {
            _dbNewspapers.AdPlacements.Add(adPlacement);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteAdPlacement(int id)
        {
            throw new NotImplementedException();
        }

        public List<AdPlacement> GetAllAdPlacements()
        {
            return _dbNewspapers.AdPlacements.ToList();
        }

        public void UpdateAdPlacement(int id, AdPlacement adPlacement)
        {
            var AdPlacementToEdit = _dbNewspapers.AdPlacements.FirstOrDefault(x => x.PlaceId == id);
            if (AdPlacementToEdit != null)
            {
                AdPlacementToEdit.PlacePrice = adPlacement.PlacePrice;
                AdPlacementToEdit.PlaceName = adPlacement.PlaceName;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
