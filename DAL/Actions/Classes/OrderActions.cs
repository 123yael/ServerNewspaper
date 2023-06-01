using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class OrderActions : IOrderActions
    {
        NewspaperSystemContext _dbNewspapers;

        public OrderActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewOrder(Order order)
        {
            _dbNewspapers.Orders.Add(order);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            throw new NotImplementedException();
        }

        public List<Order> GetAllOrders()
        {
            return _dbNewspapers.Orders.ToList();
        }

        public void UpdateOrder(int id, Order order)
        {
            var OrderToEdit = _dbNewspapers.Orders.FirstOrDefault(x => x.OrderId == id);
            if (OrderToEdit != null)
            {
                OrderToEdit.CustId = order.CustId;
                OrderToEdit.OrderFinalPrice = order.OrderFinalPrice;
                OrderToEdit.OrderDate = order.OrderDate;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
