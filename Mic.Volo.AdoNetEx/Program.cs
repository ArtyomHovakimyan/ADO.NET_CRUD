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
            string sqlExpression = "Insert into Users(Name,Age) Values('Name1',21)";
            using (SqlConnection connection = new SqlConnection(connectoinString))
            {
                connection.Open();
                Console.WriteLine("connected");
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int number = command.ExecuteNonQuery();
                Console.WriteLine("added objects: {0}", number);

            }

            Console.WriteLine("Close connect...");
        }
    }
}
