namespace iTunesHall_j
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Connection
            CustomerRepository repository = new CustomerRepository(GetConnectionString());
            static string GetConnectionString()
            {
                var builder = new SqlConnectionStringBuilder
                {
                    DataSource = "localhost\\SQLEXPRESS",
                    InitialCatalog = "Chinook",
                    IntegratedSecurity = true,
                    TrustServerCertificate = true
                };

                return builder.ConnectionString;
            }
            #endregion

            SelectLoop(repository);
        }

        /// <summary>
        /// Loop responsible for displaying the selected option.
        /// </summary>
        /// <param name="repository"></param>
        private static void SelectLoop(CustomerRepository repository)
        {
            Console.WriteLine("0 : Delete Entry By ID\n" +
                              "1 : Get All Customers\n" +
                              "2 : Get Customer by ID\n" +
                              "3 : Get Customer by Partial Name\n" +
                              "4 : Get a Page of Customers\n" +
                              "5 : Insert Customer\n" +
                              "6 : Update existing Customer\n" +
                              "7 : Get count of customers in each country\n" +
                              "8 : Get Customers by their spendings\n" +
                              "9 : Get Genres for specific customer by ID\n\n" +
                              "Customer requirement #0-9, write a number: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INSERT ID TO DELETE ENTRY: \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    int deleteId = int.Parse(Console.ReadLine());
                    repository.DeleteById(deleteId);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("DELETE COMPLETE");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "1":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("GET ALL: \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.GetAllCustomers().ToList().ForEach(c => Console.WriteLine(c));
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEND OF GET ALL \n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    SelectLoop(repository);
                    break;

                case "2":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INSERT ID OF CUSTOMER TO GET: \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    int id = int.Parse(Console.ReadLine());
                    Console.WriteLine(repository.GetCustomerById(id));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEND OF GET BY ID \n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "3":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INSERT FIRST NAME: \n");
                    string findFirstName = Console.ReadLine();
                    Console.WriteLine("INSERT LAST NAME: \n");
                    string findLastName = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.GetCustomerByName(findFirstName, findLastName).ToList().ForEach(c => Console.WriteLine(c)); ;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEND OF GET BY NAME \n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "4":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INSERT LIMIT: \n");
                    int limit = int.Parse(Console.ReadLine());
                    Console.WriteLine("INSERT OFFSET: \n");
                    int offset = int.Parse(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.GetCustomersInRange(limit, offset).ToList().ForEach(c => Console.WriteLine(c));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEND CUSTOMER RANGE \n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "5":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INSERT Name: \n");
                    string firstName = Console.ReadLine();
                    Console.WriteLine("INSERT Last Name: \n");
                    string lastName = Console.ReadLine();
                    Console.WriteLine("INSERT Country: \n");
                    string country = Console.ReadLine();
                    Console.WriteLine("INSERT Postal Code: \n");
                    string postal = Console.ReadLine();
                    Console.WriteLine("INSERT Phone: \n");
                    string phone = Console.ReadLine();
                    Console.WriteLine("INSERT Email: \n");
                    string email = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.AddCustomer(new Customer(0, firstName, lastName, country, postal, phone, email));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEND NEW CUSTOMER \n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "6":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("INSERT ID of Customer to be updated: \n");
                    int updateId = int.Parse(Console.ReadLine());
                    Console.WriteLine("UPDATE VALUES: \n");
                    Console.WriteLine("INSERT Name: \n");
                    string updateFirstName = Console.ReadLine();
                    Console.WriteLine("INSERT Last Name: \n");
                    string updateLastName = Console.ReadLine();
                    Console.WriteLine("INSERT Country: \n");
                    string updateCountry = Console.ReadLine();
                    Console.WriteLine("INSERT Postal Code: \n");
                    string updatePostal = Console.ReadLine();
                    Console.WriteLine("INSERT Phone: \n");
                    string updatePhone = Console.ReadLine();
                    Console.WriteLine("INSERT Email: \n");
                    string updateEmail = Console.ReadLine();
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.UpdateCustomer(new Customer(updateId, updateFirstName, updateLastName, updateCountry, updatePostal, updatePhone, updateEmail));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nEND CUSTOMER UPDATE\n\n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "7":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Customers in each country: \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.GetCustomersInCountryCount().ToList().ForEach(c => Console.WriteLine(c));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("END CUSTOMERS IN COUNTRY: \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "8":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Customers by spending: \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.GetCustomersBySpending().ToList().ForEach(c => Console.WriteLine(c));

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("END OF CUSTOMERS BY SPENDING \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                case "9":
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Customer genres: \n");
                    Console.WriteLine("INSERT ID of Customer to get genres: \n");
                    int genreCustomerId = int.Parse(Console.ReadLine());
                    Console.ForegroundColor = ConsoleColor.Gray;

                    repository.GetCustomerByGenre(genreCustomerId).ToList().ForEach(c => Console.WriteLine(c));
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("END OF CUSTOMER GENRES \n");
                    Console.ForegroundColor = ConsoleColor.Gray;

                    SelectLoop(repository);
                    break;

                default:
                    Console.WriteLine("Invalid input, pick numbers from 0-9");
                    SelectLoop(repository);
                    break;
            }
        }
    }
}