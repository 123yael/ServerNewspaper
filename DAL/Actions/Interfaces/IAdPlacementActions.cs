using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IAdPlacementActions
    {
        public List<AdPlacement> GetAllAdPlacements();

        public void AddNewAdPlacement(AdPlacement adPlacement);

        public void UpdateAdPlacement(int id, AdPlacement adPlacement);

        public void DeleteAdPlacement(int id);

        public AdPlacement GetPlacementById(int id);
    }
}
