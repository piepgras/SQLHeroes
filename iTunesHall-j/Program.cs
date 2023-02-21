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

            string input = Console.ReadLine();

            //1
            Console.WriteLine("SELECT ALL: \n");
            repository.GetAllCustomers().ToList().ForEach(c => Console.WriteLine(c));

            //2
            Console.WriteLine("SELECT BY ID: \n");
            SelectById(repository, "1");

            //3
            Console.WriteLine("SELECT BY PARTIAL NAME: \n");
            SelectByPartialName(repository, "Frank", "");

            //4
            int limit = 10;
            int offset = 20;
            Console.WriteLine(@"SELECT IN RANGE (LIMIT:{limit} OFFSET:{offset}): \n");
            SelectRange(repository, limit, offset);

            //5
            Customer customer1 = new Customer
            {
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
            repository.GetCustomersInCountryCount().ToList().ForEach(c => Console.WriteLine(c));

            //8
            repository.CustomersBySpending().ToList().ForEach(c => Console.WriteLine(c));

            //9
            repository.CustomersByGenre(1).ToList().ForEach(c => Console.WriteLine(c));

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