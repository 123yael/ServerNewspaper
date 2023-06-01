using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Interfaces
{
    public interface ICustomerActions
    {
        public List<Customer> GetAllCustomers();

        public void AddNewCustomer(Customer customer);

        public void UpdateCustomer(int id, Customer customer);

        public void DeleteCustomer(int id);
    }
}
