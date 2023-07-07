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
    public class OrderDetailActions : IOrderDetailActions
    {
        NewspaperSystemContext _dbNewspapers;

        public OrderDetailActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public int AddNewOrderDetail(OrderDetail orderDetail)
        {
            _dbNewspapers.OrderDetails.Add(orderDetail);
            _dbNewspapers.SaveChanges();
            return orderDetail.DetailsId;
        }

        public void DeleteOrderDetail(int id)
        {
            throw new NotImplementedException();
        }

        public List<OrderDetail> GetAllOrderDetails()
        {
            return _dbNewspapers.OrderDetails.Include(x => x.Size).ToList();
        }


        public void UpdateOrderDetail(int id, OrderDetail orderDetail)
        {
            var OrderDetailToEdit = _dbNewspapers.OrderDetails.FirstOrDefault(x => x.DetailsId == id);
            if (OrderDetailToEdit != null)
            {
                OrderDetailToEdit.OrderId = orderDetail.OrderId;
                OrderDetailToEdit.CategoryId = orderDetail.CategoryId;
                OrderDetailToEdit.WordCategoryId = orderDetail.WordCategoryId;
                OrderDetailToEdit.AdContent = orderDetail.AdContent;
                OrderDetailToEdit.AdFile = orderDetail.AdFile;
                OrderDetailToEdit.SizeId = orderDetail.SizeId;
                OrderDetailToEdit.PlaceId = orderDetail.PlaceId;
                OrderDetailToEdit.AdDuration = orderDetail.AdDuration;
                _dbNewspapers.SaveChanges();
            }
        }

        public OrderDetail GetOrderDetailsById(int? id)
        {
            OrderDetail orderDetail = GetAllOrderDetails().FirstOrDefault(x => x.DetailsId == id);
            if (orderDetail == null)
                return null;
            else
                return orderDetail;
        }
    }
}
