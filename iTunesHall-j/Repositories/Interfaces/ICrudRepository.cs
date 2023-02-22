using iTunesHall_j.Models;

namespace iTunesHall_j.Repositories.Interfaces
{
    public interface ICrudRepository<T, Id>
    {
        /// <summary>
        /// Requirement 1:
        /// Retrives all customer instances from the database.
        /// </summary>
        /// <returns>IEnumerable<T></returns>
        IEnumerable<T> GetAllCustomers();

        /// <summary>
        /// Requirement 2:
        /// Retrives a particular instance from the database by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer</returns>
        T GetCustomerById(Id id);
        
        /// <summary>
        /// Requirement 5:
        /// Inserts a new row into the database based on the parameter.
        /// </summary>
        /// <param name="entity"></param>
        void AddCustomer(T entity);
        
        /// <summary>
        /// Requirement 6:
        /// Updates an existing row based on the provided parameters.
        /// </summary>
        /// <param name="entity"></param>
        void UpdateCustomer(T entity);
        
        /// <summary>
        /// Deletes a record by its ID.
        /// </summary>
        /// <param name="id"></param>
        void DeleteById(Id id);
    }
}
