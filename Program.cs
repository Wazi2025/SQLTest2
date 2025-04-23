using System;
using System.Collections;
using Microsoft.Data.SqlClient;


namespace SQLTest2;

class Program
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

    }

    static void Main(string[] args)
    {
        //Instantiate Person object
        Person person = new Person();

        ////person is the table
        string query = "SELECT * FROM person";
        string insertQuery = "INSERT INTO person (first_name, last_name, phone, email, street, city, zip_code, country) VALUES (@f_name, @l_name, @phone, @email, @street, @city, @zip, @country)";

        //TestDB is the one we created via SQL Server Management Studio (SSMS)         
        string connectionString = "Server=localhost\\SQLEXPRESS;Database=TestDB;Trusted_Connection=True;TrustServerCertificate=true";

        bool running = true;

        while (running)
        {
            Console.WriteLine("1. SELECT");
            Console.WriteLine("2. INSERT");
            Console.WriteLine("3. Exit");
            string input = Console.ReadLine();


            //By using the 'using' statement we ensure the SQL objects are released when we are done with them
            //It also calls the Close() method before disposal
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                //Open the DB connection            
                conn.Open();

                //Choose based on user choice
                switch (input)
                {
                    //Select
                    case "1":
                        {
                            using var select = new SqlCommand(query, conn);
                            using var reader = select.ExecuteReader();

                            while (reader.Read())
                                Console.WriteLine($"{reader["person_id"]} \t{reader["first_name"]} \t{reader["last_name"]} \t{reader["phone"]} \t{reader["email"]} \t{reader["street"]} \t{reader["city"]} \t{reader["zip_code"]} \t{reader["country"]}");

                            //add some space
                            Console.WriteLine();
                            break;
                        }
                    //Insert 
                    case "2":
                        string userInsert;
                        string temp;
                        {
                            using var insert = new SqlCommand(insertQuery, conn);

                            Console.WriteLine("First name: ");
                            userInsert = Console.ReadLine();
                            person.FirstName = userInsert;
                            Console.WriteLine("Last name: ");
                            userInsert = Console.ReadLine();
                            person.LastName = userInsert;
                            Console.WriteLine("Phone: ");
                            userInsert = Console.ReadLine();
                            person.Phone = userInsert;
                            Console.WriteLine("Email: ");
                            userInsert = Console.ReadLine();
                            person.Email = userInsert;
                            Console.WriteLine("Street name: ");
                            userInsert = Console.ReadLine();
                            person.Street = userInsert;
                            Console.WriteLine("City: ");
                            userInsert = Console.ReadLine();
                            person.City = userInsert;
                            Console.WriteLine("Zip code (max 5 digits): ");

                            //Max 5 char in TestDB
                            temp = Console.ReadLine();
                            userInsert = temp.Remove(5);

                            person.ZipCode = userInsert;
                            Console.WriteLine("Country: ");
                            userInsert = Console.ReadLine();
                            person.Country = userInsert;

                            //Add to SQL insert
                            insert.Parameters.AddWithValue("@f_name", person.FirstName);
                            insert.Parameters.AddWithValue("@l_name", person.LastName);
                            insert.Parameters.AddWithValue("@phone", person.Phone);
                            insert.Parameters.AddWithValue("@email", person.Email);
                            insert.Parameters.AddWithValue("@street", person.Street);
                            insert.Parameters.AddWithValue("@city", person.City);
                            insert.Parameters.AddWithValue("@zip", person.ZipCode);
                            insert.Parameters.AddWithValue("@country", person.Country);

                            //Run query
                            insert.ExecuteNonQuery();
                            break;
                        }
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Please choose option 1, 2 or 3");
                        break;
                }

            } //SqlConnection block
            // if (input == "3")
            // {
            //     running = false;
            //     break;
            // }
        }//end While
    }
}
