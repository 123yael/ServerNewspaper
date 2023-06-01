using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IOrderActions
    {
        public List<Order> GetAllOrders();

        public void AddNewOrder(Order order);

        public void UpdateOrder(int id, Order order);

        public void DeleteOrder(int id);
    }
}
