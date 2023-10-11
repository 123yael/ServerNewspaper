using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IDatesForOrderDetailActions
    {
        public List<DatesForOrderDetail> GetAllDatesForOrderDetails();

        public void AddNewDateForOrderDetail(DatesForOrderDetail dateForOrderDetail);

        public void UpdateDateForOrderDetail(int id, DatesForOrderDetail dateForOrderDetail);

        public void DeleteDateForOrderDetail(int id);

        public void UpdateStatus(int id, bool status);
    }
}
