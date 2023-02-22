using CreateAndAccessDatabaseFjy.Models;
using iTunesHall_j.Models;

namespace iTunesHall_j.Repositories.Interfaces
{
    internal interface ICustomerRepository : ICrudRepository<Customer, int>
    {
        /// <summary>
        /// Requirement 3:
        /// Gets Customer by name.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>IEnumerable<Customer></returns>
        IEnumerable<Customer> GetCustomerByName(string firstName, string lastName);

        /// <summary>
        /// Requirement 4:
        /// Gets page of customers.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="rows"></param>
        /// <returns>IEnumerable<Customer></returns>
        IEnumerable<Customer> GetCustomersInRange(int offset, int rows);


        /// <summary>
        /// Requirement 7:
        /// Gets count of customers from each country
        /// </summary>
        /// <returns>IEnumerable<CustomerCountry></returns>
        IEnumerable<CustomerCountry> GetCustomersInCountryCount();

        /// <summary>
        /// Requirement 8:
        /// Gets customers by spending in descending order
        /// </summary>
        /// <returns>IEnumerable<CustomerSpender></returns>
        IEnumerable<CustomerSpender> GetCustomersBySpending();

        /// <summary>
        /// Requirement 9:
        /// Gets genres for a single customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IEnumerable<CustomerGenre></returns>
        IEnumerable<CustomerGenre> GetCustomerByGenre(int id);
    }
}
