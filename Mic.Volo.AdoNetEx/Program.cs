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

            string sqlExpression = "SELECT COUNT(*) FROM Users";
            using (SqlConnection connection = new SqlConnection(connectoinString))
            {
                connection.Open();
                Console.WriteLine("connected");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                object count = command.ExecuteScalar();
                command.CommandText = "SELECT MIN(Age) FROM Users";
                object minAge = command.ExecuteScalar();
                Console.WriteLine("Count is {0}",count);
                Console.WriteLine("Min value is {0}",minAge);
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
