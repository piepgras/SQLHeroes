using iTunesHall_j.Models;
using iTunesHall_j.Repositories.Implementations;
using iTunesHall_j.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace iTunesHall_j
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ICustomerRepository repository = new CustomerRepository();

            //1
            SelectAll(repository);
            //2
            SelectById(repository, "1");
            //3
            SelectByPartialName(repository, "Frank", "");
            //4
            SelectRange(repository, 10, 20);
            //5
            Customer customer1 = new Customer {
                FirstName = "Michael",
                LastName = "Neergaard",
                Country = "Denmark",
                PostalCode = "2500",
                Phone = "61676008",
                Email = "illegal"
            };
            CreateCustomer(repository, customer1);
            //6
            Customer customer2 = new Customer
            {
                CustomerID = "62",
                FirstName = "Michael Piepgras",
                LastName = "Neergaard",
                Country = "Denmark",
                PostalCode = "2500",
                Phone = "61676008",
                Email = "illegal"
            };
            UpdateCustomer(repository, customer2);
            //7
            GetCustomersByCountry(repository);
        }

        static void GetCustomersByCountry(ICustomerRepository repository)
        {
            Dictionary<string, int> customerCountByCountry = repository.GetCustomerCountByCountry();

            foreach (KeyValuePair<string, int> kvp in customerCountByCountry)
            {
                Console.WriteLine("{0} : {1}", kvp.Key, kvp.Value);
            }
        }

        static void UpdateCustomer(ICustomerRepository repository, Customer customer)
        {
            repository.UpdateCustomer(customer);
        }

        static void CreateCustomer(ICustomerRepository repository, Customer customer)
        {
            repository.AddCustomer(customer);
        }

        static void SelectRange(ICustomerRepository repository, int limit, int offset)
        {
            printCustomers(repository.GetCustomersInRange(limit, offset));
        }

        static void SelectAll(ICustomerRepository repository)
        {
            printCustomers(repository.GetAllCustomers());
        }

        static void SelectById(ICustomerRepository repository, string id)
        {
            printCustomer(repository.GetCustomerById(id));
        }

        static void SelectByPartialName(ICustomerRepository repository, string firstName, string lastName)
        {
            printCustomer(repository.GetCustomerByName(firstName, lastName));
        }

        static void printCustomers(List<Customer> customers)
        {
            Console.WriteLine("BEGIN CUSTOMER LIST: ");
            foreach (Customer customer in customers)
            {
                printCustomer(customer);
            }
            Console.WriteLine("END CUSTOMER LIST");
        }

        static void printCustomer(Customer customer)
        {
            Console.WriteLine("ID: " + customer.CustomerID +
                              " \nNAME: " + customer.FirstName + " " + customer.LastName +
                              " \nCOUNTRY: " + customer.Country + "          ZIP: " + customer.PostalCode +
                              " \nPHONE: " + customer.Phone + " E-MAIL: " + customer.Email + "\n");
        }

    }

    public static class ConnectionString
    {
        public static string getBuilder()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "DESKTOP-0NJA0PF\\SQLEXPRESS", // localhost\\SQLExpress ?? DESKTOP-0NJA0PF\\SQLEXPRESS
                InitialCatalog = "Chinook",
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };

            return builder.ConnectionString;
        }
    }
}