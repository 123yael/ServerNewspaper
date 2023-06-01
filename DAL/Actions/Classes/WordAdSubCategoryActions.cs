using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class WordAdSubCategoryActions : IWordAdSubCategoryActions
    {
        NewspaperSystemContext _dbNewspapers;

        public WordAdSubCategoryActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewWordAdSubCategory(WordAdSubCategory wordAdSubCategory)
        {
            _dbNewspapers.WordAdSubCategories.Add(wordAdSubCategory);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteWordAdSubCategory(int id)
        {
            throw new NotImplementedException();
        }

        public List<WordAdSubCategory> GetAllWordAdSubCategories()
        {
            return _dbNewspapers.WordAdSubCategories.ToList();
        }

        public void UpdateWordAdSubCategory(int id, WordAdSubCategory wordAdSubCategory)
        {
            var WordAdSubCategoryToEdit = _dbNewspapers.WordAdSubCategories.FirstOrDefault(x => x.WordCategoryId == id);
            if (WordAdSubCategoryToEdit != null)
            {
                WordAdSubCategoryToEdit.WordCategoryName = wordAdSubCategory.WordCategoryName;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
