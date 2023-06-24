using DAL.Actions.Classes;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IAdSizeActions
    {
        public List<AdSize> GetAllAdSizes();

        public void AddNewAdSize(AdSize adSize);

        public void UpdateAdSize(int id, AdSize adSize);

        public void DeleteAdSize(int id);

        public AdSize getSizeById(int id);

    }
}
