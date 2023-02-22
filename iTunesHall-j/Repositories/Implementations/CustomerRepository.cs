using CreateAndAccessDatabaseFjy.Models;
using iTunesHall_j.Exceptions;
using iTunesHall_j.Models;
using iTunesHall_j.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace iTunesHall_j.Repositories.Implementations
{
    internal class CustomerRepository : ICustomerRepository
    {
        #region Connection
        private string ConnectionString { get; set; } = string.Empty;
        public CustomerRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        #endregion

        #region Customer requirement 1
        /// <summary>
        /// Gets all Customers from the database.
        /// Creates an SQL query which SELECTs customers from the database,
        /// getting the id, first name, last name, country, postal code and phone number.
        /// Postal code and phone number are parsed to check if they are null and returns
        /// as an empty string if they.
        /// </summary>
        /// <returnsIEnumerable<Customer><Customer></returns>
        public IEnumerable<Customer> GetAllCustomers()
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode,ISNULL(Phone,'') AS Phone, Email FROM Customer";
            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6)
                );
            }
        }
        #endregion

        #region Customer requirement 2
        /// <summary>
        /// Gets a single Customer, specified by ID, from the database.
        /// Creates an SQL query which SELECTs a single customer from the database,
        /// WHERE CustomerId is equals to the input id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Customer</returns>
        public Customer GetCustomerById(int id)
        {
            Customer customer = new Customer();
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT CustomerId , FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode,ISNULL(Phone,'') AS Phone , Email " +
                           "FROM Customer " +
                          "WHERE CustomerId = @CustomerId";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CustomerId", id);
            using SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    customer = new Customer(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6)
                    );
                }
            }
            else
            {
                throw new CustomerNotFoundException("No customer exists with that ID");
            }
            return customer;
        }
        #endregion

        #region Customer requirement 3
        /// <summary>
        /// Gets a single Customer, specified by a partial or impartial name from the database.
        /// Creates an SQL query which SELECTs a single customer from the database,
        /// WHERE the FirstName and LastName are LIKE the name selected by the database.
        /// Uses the % wildcard character which is used to match any sequence of characters in any partition of a string. 
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>IEnumerable<Customer></returns>
        public IEnumerable<Customer> GetCustomerByName(string firstName, string lastName)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode, ISNULL(Phone,'') AS Phone, Email" +
                         " FROM Customer" +
                         " WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@FirstName", "%" + firstName + "%");
            command.Parameters.AddWithValue("@LastName", "%" + lastName + "%") ;
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6)
                );
            }
        }
        #endregion

        #region Customer requirement 4
        /// <summary>
        /// Get a specified range of Customers from the database.
        /// Creates an SQL query which recieves a, in ascending order, list of customers
        /// Uses the OFFSET and FETCH NEXT clauses.
        /// OFFSET indicates how many rows should be skipped.
        /// FETCH NEXT indicates how many rows should be returned.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>IEnumerable<Customer></returns>
        public IEnumerable<Customer> GetCustomersInRange(int limit, int offset)
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode, ISNULL(Phone,'') AS Phone, Email " +
                         "FROM Customer " +
                         "ORDER BY CustomerId " +
                         "OFFSET @Offset ROWS " +
                         "FETCH NEXT @Limit ROWS ONLY";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Offset", offset);
            command.Parameters.AddWithValue("@Limit", limit);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new Customer(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    reader.GetString(6)
                );
            }
        }
        #endregion

        #region Customer requirement 5
        /// <summary>
        /// Creates an INSERT INTO statement SQL query.
        /// Inserts a Customer into the database.
        /// Uses the VALUES clause where the specificed values that need
        /// to be inserted are specified by @.
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {
            string sql = "INSERT INTO Customer (FirstName, LastName, Country, PostalCode, Phone, Email) " +
                         "VALUES (@FirstName, @LastName, @Country, @PostalCode, @Phone, @Email)";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                        command.Parameters.AddWithValue("@Country", customer.Country);
                        command.Parameters.AddWithValue("@PostalCode", string.IsNullOrEmpty(customer.PostalCode) ? (object)DBNull.Value : customer.PostalCode);
                        command.Parameters.AddWithValue("@Phone", string.IsNullOrEmpty(customer.Phone) ? (object)DBNull.Value : customer.Phone);
                        command.Parameters.AddWithValue("@Email", customer.Email);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }
        }
        #endregion

        #region Customer requirement 6
        /// <summary>
        /// Updates and existing Customer in the database, with new values.
        /// Uses the UPDATE statement to indicate that a Customer is to be updated.
        /// The Customer is specified by id in the WHERE.
        /// Uses SET and @ to specify what is meant to be updated and with what.
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            using SqlConnection connection = new(ConnectionString);
            connection.Open();
            string sql = "UPDATE Customer " +
                         "SET FirstName = @FirstName, LastName = @LastName, Country = @Country, PostalCode = @PostalCode, Phone = @Phone, Email = @Email " +
                         "WHERE CustomerId = @CustomerId";

            using SqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@CustomerId", customer.CustomerId);
            command.Parameters.AddWithValue("@FirstName", customer.FirstName);
            command.Parameters.AddWithValue("@LastName", customer.LastName);
            command.Parameters.AddWithValue("@Country", customer.Country);
            command.Parameters.AddWithValue("@PostalCode", customer.PostalCode);
            command.Parameters.AddWithValue("@Phone", customer.Phone);
            command.Parameters.AddWithValue("@Email", customer.Email);

            command.ExecuteNonQuery();
        }
        #endregion

        #region Customer requirement 7
        /// <summary>
        /// Gets the amount of Customers from each country.
        /// SELECTs the Country coloumn from the database and creates a Count to track Customers in each country.
        /// COUNT aggregates numbers from matching country value (same country).
        /// ORDER BY the count in descending order.
        /// </summary>
        /// <returns>IEnumerable<CustomerCountry><string, int></returns>
        public IEnumerable<CustomerCountry> GetCustomersInCountryCount()
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "SELECT Country, COUNT(*) AS Count " +
                         "FROM Customer " +
                         "GROUP BY Country " +
                         "ORDER BY Count DESC";
            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new CustomerCountry(reader.GetString(0), reader.GetInt32(1));
            }
        }
        #endregion

        #region Customer requirement 8
        /// <summary>
        /// Gets Customers by spending in descending order.
        /// SELECTs coloumns from Customer Table
        /// SUMs the total amount of invoices
        /// GROUP specifies Invoice as the primary table and JOINs customer on to it.
        /// ORDERs in descending order
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerSpender> GetCustomersBySpending()
        {
            using var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var sql = " SELECT t2.CustomerId, t2.FirstName, t2.LastName, SUM(Total) AS TotalInvoice " +
                      " FROM Invoice t1 JOIN Customer t2 ON t1.CustomerId= t2.CustomerId" +
                      " GROUP BY t2.CustomerId, t2.FirstName, t2.LastName " +
                      " ORDER BY SUM(Total) desc";
            using var command = new SqlCommand(sql, connection);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                yield return new CustomerSpender(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetDecimal(3)
                );
            }
        }
        #endregion

        #region Customer requirement 9
        /// <summary>
        /// Retrieves the top 5 customers who purchased tracks of a specific genre,
        /// along with the total count of tracks bought for that genre, for a given customer ID.
        /// Uses several table JOINs and a GROUP BY clause to aggregate data.
        /// WITH TIES clause is used to include any additional rows that have the same count as the last row
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CustomerGenre> GetCustomerByGenre(int id)
        {
            CustomerGenre customer = new CustomerGenre();
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = " SELECT TOP 5 WITH TIES t5.CustomerId, t5.FirstName,t5.LastName, t1.Name , COUNT(t1.Name)" +
                         " FROM Genre t1 " +
                         " INNER JOIN Track       t2 ON t1.GenreId = t2.GenreId " +
                         " INNER JOIN InvoiceLine t3 ON t2.TrackId = t3.TrackId " +
                         " INNER JOIN Invoice     t4 ON t3.InvoiceId = t4.InvoiceId " +
                         " INNER JOIN Customer    t5 ON t4.CustomerId = t5.CustomerId " +
                         " WHERE t5.CustomerId = @CustomerId " +
                         " GROUP BY t5.CustomerId,t5.FirstName,t5.LastName , t1.Name" +
                         " ORDER BY COUNT(t1.Name) DESC ";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CustomerId", id);
            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return customer = new CustomerGenre(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3)
                    );
            }
        }
        #endregion

        #region Delete Entry
        /// <summary>
        /// Deletes an entry by ID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(int id)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            string sql = "DELETE FROM Customer WHERE CustomerId = @CustomerId";
            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@CustomerId", id);
            using SqlDataReader reader = command.ExecuteReader();
        }
        #endregion
    }
}
