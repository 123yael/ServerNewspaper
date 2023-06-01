using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IAdvertisementCategoryActions
    {
        public List<AdvertisementCategory> GetAllAdvertisementCategories();

        public void AddNewAdvertisementCategory(AdvertisementCategory advertisementCategory);

        public void UpdateAdvertisementCategory(int id, AdvertisementCategory advertisementCategory);

        public void DeleteAdvertisementCategory(int id);
    }
}
