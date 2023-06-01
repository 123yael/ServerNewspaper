using DAL.Actions.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Actions.Classes
{
    public class CustomerActions : ICustomerActions
    {
        NewspaperSystemContext _dbNewspapers;

        public CustomerActions(NewspaperSystemContext dbNewspapers)
        {
            this._dbNewspapers = dbNewspapers;
        }

        public void AddNewCustomer(Customer customer)
        {
            _dbNewspapers.Customers.Add(customer);
            _dbNewspapers.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAllCustomers()
        {
            return _dbNewspapers.Customers.ToList();
        }

        public void UpdateCustomer(int id, Customer customer)
        {
            var CustomerToEdit = _dbNewspapers.Customers.FirstOrDefault(x => x.CustId == id);
            if (CustomerToEdit != null)
            {
                CustomerToEdit.CustFirstName = customer.CustFirstName;
                CustomerToEdit.CustLastName = customer.CustLastName;
                CustomerToEdit.CustEmail = customer.CustEmail;
                CustomerToEdit.CustPhone = customer.CustPhone;
                CustomerToEdit.CustPassword = customer.CustPassword;
                _dbNewspapers.SaveChanges();
            }
        }
    }
}
