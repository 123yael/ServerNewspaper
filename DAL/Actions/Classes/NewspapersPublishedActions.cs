using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class NewspapersPublishedActions : INewspapersPublishedActions
    {
        NewspaperSystemContext _dbNewspapers;

        public NewspapersPublishedActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewNewspaperPublished(NewspapersPublished newspaperPublished)
        {
            _dbNewspapers.NewspapersPublisheds.Add(newspaperPublished);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteNewspaperPublished(int id)
        {
            throw new NotImplementedException();
        }

        public List<NewspapersPublished> GetAllNewspapersPublished()
        {
            return _dbNewspapers.NewspapersPublisheds.ToList();
        }

        public void UpdateNewspaperPublished(int id, NewspapersPublished newspaperPublished)
        {
            var NewspaperPublishedToEdit = _dbNewspapers.NewspapersPublisheds.FirstOrDefault(x => x.NewspaperId == id);
            if (NewspaperPublishedToEdit != null)
            {
                NewspaperPublishedToEdit.PublicationDate = newspaperPublished.PublicationDate;
                NewspaperPublishedToEdit.CountPages = newspaperPublished.CountPages;
                NewspaperPublishedToEdit.MagazineNumber = newspaperPublished.MagazineNumber;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
