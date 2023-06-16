using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface IOrderDetailActions
    {
        public List<OrderDetail> GetAllOrderDetails();

        public int AddNewOrderDetail(OrderDetail orderDetail);

        public void UpdateOrderDetail(int id, OrderDetail orderDetail);

        public void DeleteOrderDetail(int id);
        public OrderDetail GetOrderDetailsById(int? id);
    }
}
