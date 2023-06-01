using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IWordAdSubCategoryActions
    {
        public List<WordAdSubCategory> GetAllWordAdSubCategories();

        public void AddNewWordAdSubCategory(WordAdSubCategory wordAdSubCategory);

        public void UpdateWordAdSubCategory(int id, WordAdSubCategory wordAdSubCategory);

        public void DeleteWordAdSubCategory(int id);
    }
}
