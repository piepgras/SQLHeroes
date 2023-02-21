using iTunesHall_j.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTunesHall_j.Repositories.Interfaces
{
    internal interface ICustomerRepository
    { 
        // 1
        public List<Customer> GetAllCustomers();

        // 2
        public Customer GetCustomerById(string id);

        // 3
        public Customer GetCustomerByName(string firstName, string lastName);

        // 4
        public List<Customer> GetCustomersInRange(int limit, int offset);

        // 5
        public void AddCustomer(Customer customer);

        // 6
        public void UpdateCustomer(Customer customer);

        // 7
        public IEnumerable<CustomerCountry> GetCustomersInCountryCount();

        // 8
        public IEnumerable<CustomerSpender> CustomersBySpending();

        // 9
        public IEnumerable<CustomerGenre> CustomersByGenre(int id);

    }
}
