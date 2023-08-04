using DAL.Actions.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public  class DatesForOrderDetailActions : IDatesForOrderDetailActions
    {
        NewspaperSystemContext _dbNewspapers;

        public DatesForOrderDetailActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewDateForOrderDetail(DatesForOrderDetail dateForOrderDetail)
        {
            _dbNewspapers.DatesForOrderDetails.Add(dateForOrderDetail);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteDateForOrderDetail(int id)
        {
            throw new NotImplementedException();
        }

        public List<DatesForOrderDetail> GetAllDatesForOrderDetails()
        {
            return _dbNewspapers.DatesForOrderDetails.Include(x => x.Details.Order.Cust).Include(x => x.Details.Size).ToList();
        }

        public void UpdateDateForOrderDetail(int id, DatesForOrderDetail dateForOrderDetail)
        {
            var OrderDetailToEdit = _dbNewspapers.DatesForOrderDetails.FirstOrDefault(x => x.DateId == id);
            if (OrderDetailToEdit != null)
            {
                OrderDetailToEdit.DetailsId = dateForOrderDetail.DetailsId;
                OrderDetailToEdit.Date = dateForOrderDetail.Date;
                OrderDetailToEdit.DateStatus = dateForOrderDetail.DateStatus;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
