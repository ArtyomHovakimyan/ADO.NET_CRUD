using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace Mic.Volo.AdoNetEx
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectoinString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Console.WriteLine("Enter name:");
            string name = Console.ReadLine();
            Console.WriteLine("Enter age:");
            int age = int.Parse(Console.ReadLine());
            string sqlExpression = string.Format("INSERT INTO Users (Name,Age) VALUES ('{0}','{1}')", name, age);
            using (SqlConnection connection = new SqlConnection(connectoinString))
            {
                connection.Open();
                Console.WriteLine("connected");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int count = command.ExecuteNonQuery();
                Console.WriteLine("Added {0} objects.", count);

                name = Console.ReadLine();
                sqlExpression = string.Format("UPDATE Users SET Name='{0}' WHERE Agge={1}", name, age);
                command.CommandText = sqlExpression;
                count = command.ExecuteNonQuery();
                Console.WriteLine("Update {0} objects:", count);

            }


            Console.WriteLine("Close connect...");

        }
        private static async Task ReadDataAsync()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            string sqlExpression = "SELECT * FROM Users";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}", reader.GetName(0), reader.GetName(1), reader.GetName(2));

                    while (await reader.ReadAsync())
                    {
                        object id = reader.GetValue(0);
                        object name = reader.GetValue(1);
                        object age = reader.GetValue(2);
                        Console.WriteLine("{0} \t{1} \t{2}", id, name, age);
                    }
                }
                reader.Close();
            }
        }
        //private static async Task ReadDataAsync()
        //{
        //    string connectoinString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //    string sqlExpression = "SELECT * FROM Users";
        //    using (SqlConnection connection=new SqlConnection(connectoinString))
        //    {
        //        await connection.OpenAsync();
        //        SqlCommand command = new SqlCommand(sqlExpression, connection);
        //        SqlDataReader reader = await command.ExecuteReaderAsync();
        //        if(reader.HasRows)
        //        {
        //            Console.WriteLine("{0}\t{1}\t{2}",reader.GetName(0),reader.GetName(1),reader.GetName(2));
        //            while (await reader.ReadAsync())
        //            {
        //                object id = reader.GetValue(0);
        //                object name = reader.GetValue(1);
        //                object age = reader.GetValue(2);
        //                Console.WriteLine("{0}\t{1}\t{2}",id,name,age);
        //            }
        //            reader.Close();
        //        }
        //    }
        //}
    }
}
