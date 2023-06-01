using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class AdSizeActions : IAdSizeActions
    {
        NewspaperSystemContext _dbNewspapers;

        public AdSizeActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewAdSize(AdSize adSize)
        {
            _dbNewspapers.AdSizes.Add(adSize);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteAdSize(int id)
        {
            throw new NotImplementedException();
        }

        public List<AdSize> GetAllAdSizes()
        {
            return _dbNewspapers.AdSizes.ToList();
        }

        public void UpdateAdSize(int id, AdSize adSize)
        {
            var AdSizeToEdit = _dbNewspapers.AdSizes.FirstOrDefault(x => x.SizeId == id);
            if (AdSizeToEdit != null)
            {
                AdSizeToEdit.SizeName = adSize.SizeName;
                AdSizeToEdit.SizePrice = adSize.SizePrice;
                AdSizeToEdit.SizeWidth = adSize.SizeWidth;
                AdSizeToEdit.SizeHeight = adSize.SizeHeight;
                AdSizeToEdit.SizeImg = adSize.SizeImg;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
