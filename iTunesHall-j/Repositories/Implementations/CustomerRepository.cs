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
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customerList = new List<Customer>();
            string sql = "SELECT CustomerId , FirstName, LastName, Country, ISNULL(PostalCode,'') AS PostalCode,ISNULL(Phone,'') AS Phone , Email FROM Customer";

            try
            {
                using(SqlConnection connection = new SqlConnection(ConnectionString.getBuilder()))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Customer temp = new Customer();

                                temp.CustomerID =   reader.GetInt32(0).ToString();
                                temp.FirstName  =   reader.GetString(1);
                                temp.LastName   =   reader.GetString(2);
                                temp.Country    =   reader.GetString(3);
                                temp.PostalCode =   reader.GetString(4);
                                temp.Phone      =   reader.GetString(5);
                                temp.Email      =   reader.GetString(6);

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
        public Dictionary<string, int> GetCustomerCountByCountry()
        {
            Dictionary<string, int> countryCounts = new Dictionary<string, int>();
            string sql = "SELECT Country, COUNT(*) AS Count " +
                         "FROM Customer " +
                         "GROUP BY Country " +
                         "ORDER BY Count DESC";

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
                                string country = reader.GetString(0);
                                int count = reader.GetInt32(1);
                                countryCounts.Add(country, count);
                            }
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw;
            }

            return countryCounts;
        }
        public void CustomersBySpending()
        {
            throw new NotImplementedException();
        }
    }
}
