using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IPagesInNewspaperActions
    {
        public List<PagesInNewspaper> GetAllPagesInNewspapers();

        public void AddNewPageInNewspaper(PagesInNewspaper pageInNewspaper);

        public void UpdatePageInNewspaper(int id, PagesInNewspaper pageInNewspaper);

        public void DeletePageInNewspaper(int id);
    }
}
