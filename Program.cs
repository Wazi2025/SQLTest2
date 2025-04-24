using System;
using System.Collections;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;


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

    static private string ValidateInput(string consoleText, string userInsert)
    {
        while (String.IsNullOrWhiteSpace(userInsert))
        {
            Console.WriteLine(consoleText);
            userInsert = Console.ReadLine();
        }

        return userInsert;
    }
    static private void SQLSelect(SqlConnection conn)
    {
        string query = "SELECT * FROM person";

        //Instantiate SQL object with query and current connection (conn) from Main()
        using var select = new SqlCommand(query, conn);
        using var reader = select.ExecuteReader();

        //Read from DB using the query. Will continue until there are no more rows
        while (reader.Read())
            Console.WriteLine($"{reader["person_id"]} \t{reader["first_name"]} \t{reader["last_name"]} \t{reader["phone"]} \t{reader["email"]} \t{reader["street"]} \t{reader["city"]} \t{reader["zip_code"]} \t{reader["country"]}");

        //add some space
        Console.WriteLine();
    }

    static private void SQLInsert(SqlConnection conn)
    {
        //Instantiate Person object here since this is the only place it's used
        Person person = new Person();

        string insertQuery = "INSERT INTO person (first_name, last_name, phone, email, street, city, country, zip_code) VALUES (@f_name, @l_name, @phone, @email, @street, @city, @country, @zip)";
        //string insertQuery = "INSERT INTO person (first_name, last_name, email) VALUES (@f_name, @l_name, @email)";
        string userInsert = null;
        string consoleText = null;
        //{
        //Instantiate SQL object with query and current connection (conn) from Main()
        using var insert = new SqlCommand(insertQuery, conn);

        consoleText = "First name: ";
        person.FirstName = ValidateInput(consoleText, userInsert);

        consoleText = "Last name: ";
        person.LastName = ValidateInput(consoleText, userInsert);

        consoleText = "Phone: ";
        //Prolly use a number Validate method here
        person.Phone = ValidateInput(consoleText, userInsert);

        //Add some sort of email Validation
        consoleText = "Email: ";
        person.Email = ValidateInput(consoleText, userInsert);

        consoleText = "Street: ";
        person.Street = ValidateInput(consoleText, userInsert);

        consoleText = "City: ";
        person.City = ValidateInput(consoleText, userInsert);

        //Use number + max length > 5 Validate method
        Console.WriteLine("Zip code (max 5 digits): ");
        userInsert = Console.ReadLine();

        //Max 5 char in TestDB
        if (userInsert.Length > 5)
        {
            userInsert = userInsert.Remove(5);
        }
        person.ZipCode = userInsert;

        //Setting this variable temporarily to null so person.Country's Validate check isn't skipped
        //
        userInsert = null;

        consoleText = "Country: ";
        person.Country = ValidateInput(consoleText, userInsert);

        //Add to SQL insert
        insert.Parameters.AddWithValue("@f_name", person.FirstName);
        insert.Parameters.AddWithValue("@l_name", person.LastName);
        insert.Parameters.AddWithValue("@phone", person.Phone);
        insert.Parameters.AddWithValue("@email", person.Email);
        insert.Parameters.AddWithValue("@street", person.Street);
        insert.Parameters.AddWithValue("@city", person.City);
        insert.Parameters.AddWithValue("@country", person.Country);
        insert.Parameters.AddWithValue("@zip", person.ZipCode);

        //Run INSERT query
        insert.ExecuteNonQuery();
        //}
    }

    static void Main(string[] args)
    {
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
                    case "1":
                        //Select
                        SQLSelect(conn);
                        break;
                    case "2":
                        //Insert 
                        SQLInsert(conn);
                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Please choose option 1, 2 or 3");
                        break;
                }
            }
        }//End While
    }//End Main
}
