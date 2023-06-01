using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class AdvertisementCategoryActions : IAdvertisementCategoryActions
    {
        NewspaperSystemContext _dbNewspapers;

        public AdvertisementCategoryActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewAdvertisementCategory(AdvertisementCategory advertisementCategory)
        {
            _dbNewspapers.AdvertisementCategories.Add(advertisementCategory);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteAdvertisementCategory(int id)
        {
            throw new NotImplementedException();
        }

        public List<AdvertisementCategory> GetAllAdvertisementCategories()
        {
            return _dbNewspapers.AdvertisementCategories.ToList();
        }

        public void UpdateAdvertisementCategory(int id, AdvertisementCategory advertisementCategory)
        {
            var AdvertisementCategoryToEdit = _dbNewspapers.AdvertisementCategories.FirstOrDefault(x => x.CategoryId == id);
            if (AdvertisementCategoryToEdit != null)
            {
                AdvertisementCategoryToEdit.CategoryName = advertisementCategory.CategoryName;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
