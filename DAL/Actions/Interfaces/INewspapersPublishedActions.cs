using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface INewspapersPublishedActions
    {
        public List<NewspapersPublished> GetAllNewspapersPublished();

        public void AddNewNewspaperPublished(NewspapersPublished newspaperPublished);

        public void UpdateNewspaperPublished(int id, NewspapersPublished newspaperPublished);

        public void DeleteNewspaperPublished(int id);
    }
}
