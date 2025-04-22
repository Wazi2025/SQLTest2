using System;
using System.Collections;
using Microsoft.Data.SqlClient;


namespace SQLTest2;

class Program
{
    static void Main(string[] args)
    {
        ////person is the table
        string query = "SELECT * FROM person";
        string insertQuery = "INSERT INTO person (first_name, last_name, phone, email, street, city, country, zip_code) VALUES (@f_name, @l_name, @phone, @email, @street, @country, @city, @zip)";

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

                //Instantiate SELECT and INSERT SQL Objects
                // using var select = new SqlCommand(query, conn);
                // using var insert = new SqlCommand(query, conn);

                // using var reader = select.ExecuteReader();

                //Choose based on user choice
                switch (input)
                {
                    //Select
                    case "1":
                        {
                            using var select = new SqlCommand(query, conn);
                            using var reader = select.ExecuteReader();

                            while (reader.Read())
                                Console.WriteLine($"{reader["person_id"]} \t{reader["first_name"]} \t{reader["last_name"]} \t{reader["email"]}");

                            //add some space
                            Console.WriteLine();
                            break;
                        }
                    //Insert 
                    case "2":
                        {
                            using var insert = new SqlCommand(insertQuery, conn);

                            insert.Parameters.AddWithValue("@f_name", "John");
                            insert.Parameters.AddWithValue("@l_name", "Doe");
                            insert.Parameters.AddWithValue("@phone", "");
                            insert.Parameters.AddWithValue("@email", "john_doe@mail.com");
                            insert.Parameters.AddWithValue("@street", "Teststree 1");
                            insert.Parameters.AddWithValue("@country", "Sosaria");
                            insert.Parameters.AddWithValue("@city", "Britannia");
                            insert.Parameters.AddWithValue("@zip", "0000");

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
