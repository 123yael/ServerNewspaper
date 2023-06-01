using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class PagesInNewspaperActions : IPagesInNewspaperActions
    {
        NewspaperSystemContext _dbNewspapers;

        public PagesInNewspaperActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewPageInNewspaper(PagesInNewspaper pageInNewspaper)
        {
            _dbNewspapers.PagesInNewspapers.Add(pageInNewspaper);
            _dbNewspapers.SaveChanges();
        }

        public void DeletePageInNewspaper(int id)
        {
            throw new NotImplementedException();
        }

        public List<PagesInNewspaper> GetAllPagesInNewspapers()
        {
            return _dbNewspapers.PagesInNewspapers.ToList();
        }

        public void UpdatePageInNewspaper(int id, PagesInNewspaper pageInNewspaper)
        {
            var PageInNewspaperToEdit = _dbNewspapers.PagesInNewspapers.FirstOrDefault(x => x.PageId == id);
            if (PageInNewspaperToEdit != null)
            {
                PageInNewspaperToEdit.PageNumber = pageInNewspaper.PageNumber;
                PageInNewspaperToEdit.NewspaperId = pageInNewspaper.NewspaperId;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
