using iTunesHall_j.Models;
using iTunesHall_j.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace iTunesHall_j.Repositories.Implementations
{
    internal class CustomerRepository : ICustomerRepository
    {
        #region Customer requirement 1
        /// <summary>
        /// Gets all Customers from the database.
        /// Creates an SQL query which SELECTs customers from the database,
        /// getting the id, first name, last name, country, postal code and phone number.
        /// Postal code and phone number are parsed to check if they are null and returns
        /// as an empty string if they.
        /// </summary>
        /// <returns>List<Customer></returns>
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customerList = new List<Customer>();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode,ISNULL(Phone,'') AS Phone, Email FROM Customer";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer temp = new Customer();

                                temp.CustomerID = reader.GetInt32(0).ToString();
                                temp.FirstName = reader.GetString(1);
                                temp.LastName = reader.GetString(2);
                                temp.Country = reader.GetString(3);
                                temp.PostalCode = reader.GetString(4);
                                temp.Phone = reader.GetString(5);
                                temp.Email = reader.GetString(6);

                                customerList.Add(temp);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }

            return customerList;
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
        public Customer GetCustomerById(string id)
        {
            Customer customer = new Customer();
            string sql = "SELECT CustomerId , FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode,ISNULL(Phone,'') AS Phone , Email FROM Customer" +
                " WHERE CustomerId = @CustomerId";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customer.CustomerID = reader.GetInt32(0).ToString();
                                customer.FirstName = reader.GetString(1);
                                customer.LastName = reader.GetString(2);
                                customer.Country = reader.GetString(3);
                                customer.PostalCode = reader.GetString(4);
                                customer.Phone = reader.GetString(5);
                                customer.Email = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {

                throw;
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
        /// <returns></returns>
        public Customer GetCustomerByName(string firstName, string lastName)
        {
            Customer customer = new Customer();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode, ISNULL(Phone,'') AS Phone, Email FROM Customer " +
                         "WHERE FirstName LIKE @FirstName AND LastName LIKE @LastName";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", "%" + firstName + "%");
                        command.Parameters.AddWithValue("@LastName", "%" + lastName + "%");

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customer.CustomerID = reader.GetInt32(0).ToString();
                                customer.FirstName = reader.GetString(1);
                                customer.LastName = reader.GetString(2);
                                customer.Country = reader.GetString(3);
                                customer.PostalCode = reader.GetString(4);
                                customer.Phone = reader.GetString(5);
                                customer.Email = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }

            return customer;
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
        /// <returns>List<Customer></returns>
        public List<Customer> GetCustomersInRange(int limit, int offset)
        {
            List<Customer> customers = new List<Customer>();
            string sql = "SELECT CustomerId, FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode, ISNULL(Phone,'') AS Phone, Email " +
                         "FROM Customer " +
                         "ORDER BY CustomerId " +
                         "OFFSET @Offset ROWS " +
                         "FETCH NEXT @Limit ROWS ONLY";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Limit", limit);
                        command.Parameters.AddWithValue("@Offset", offset);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer customer = new Customer();
                                customer.CustomerID = reader.GetInt32(0).ToString();
                                customer.FirstName = reader.GetString(1);
                                customer.LastName = reader.GetString(2);
                                customer.Country = reader.GetString(3);
                                customer.PostalCode = reader.GetString(4);
                                customer.Phone = reader.GetString(5);
                                customer.Email = reader.GetString(6);

                                customers.Add(customer);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }

            return customers;
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
                using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
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
            string sql = "UPDATE Customer " +
                         "SET FirstName = @FirstName, LastName = @LastName, Country = @Country, PostalCode = @PostalCode, Phone = @Phone, Email = @Email " +
                         "WHERE CustomerId = @CustomerId";

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
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
                        command.Parameters.AddWithValue("@CustomerId", customer.CustomerID);

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

        #region Customer requirement 7
        /// <summary>
        /// Gets the amount of Customers from each country.
        /// </summary>
        /// <returns>Dictionary<string, int></returns>
        public IEnumerable<CustomerCountry> GetCustomersInCountryCount()
        {
            CustomerCountry customer = new CustomerCountry();
            string sql = "SELECT Country, COUNT(*) AS Count " +
                         "FROM Customer " +
                         "GROUP BY Country " +
                         "ORDER BY Count DESC";

            using (SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new CustomerCountry(
                                reader.GetString(0),
                                reader.GetInt32(1)
                                );
                        }
                    }
                }
            }
        }
        #endregion

        #region Customer requirement 8
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerSpender> CustomersBySpending()
        {
            using var connection = new SqlConnection(ConnectionString.getBuilder());
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<CustomerGenre> CustomersByGenre(int id)
        {
            CustomerGenre customer = new CustomerGenre();
            using SqlConnection connection = new SqlConnection(ConnectionString.getBuilder());
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
    }
}
