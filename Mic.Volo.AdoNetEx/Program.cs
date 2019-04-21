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
        static string connectoinString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        static void Main(string[] args)
        {
            using (SqlConnection connection=new SqlConnection(connectoinString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction();

                SqlCommand command = connection.CreateCommand();
                command.Transaction = transaction;
                try
                {
                    command.CommandText = "INSERT INTO Users (Name,Age) VALUES ('Tim',34)";
                    command.ExecuteNonQuery();
                    command.CommandText = "INSERT INTO Users (Name,Age) VALUES ('Kat',31)";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                    Console.WriteLine("Data added in database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                }
            }
        }
        private static void GetAgeRange(string name)
        {
            string sqlExpression = "sp_GetAgeRange";
            using (SqlConnection connection=new SqlConnection(connectoinString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);
                SqlParameter minAgeParam = new SqlParameter
                {
                    ParameterName = "@minAge",
                    SqlDbType = System.Data.SqlDbType.Int
                };
                minAgeParam.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(minAgeParam);
                SqlParameter maxAgeParam = new SqlParameter
                {
                    ParameterName = "@maxAge",
                    SqlDbType = System.Data.SqlDbType.Int
                };
                maxAgeParam.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(maxAgeParam);
                command.ExecuteNonQuery();

                Console.WriteLine("Min age : {0}",command.Parameters["@minAge"].Value);
                Console.WriteLine("Max age : {0}",command.Parameters["@maxAge"].Value);
            }
        }
        private static void AddUser(string name,int age)
        {
            string sqlExpression = "sp_InsertUser";
            using (SqlConnection connection=new SqlConnection(connectoinString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = name
                };
                command.Parameters.Add(nameParam);
                SqlParameter ageParam = new SqlParameter
                {
                    ParameterName = "@age",
                    Value = age
                };
                command.Parameters.Add(ageParam);
                var result = command.ExecuteScalar();
                Console.WriteLine("Id added object: {0}",result);
            }
        }
        private static void GetUsers()
        {
            string sqlExpression = "sp_GetUsers";
            using (SqlConnection connection=new SqlConnection(connectoinString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var reader = command.ExecuteReader();
                if(reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}",reader.GetName(0),reader.GetName(1),reader.GetName(2));
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        int age = reader.GetInt32(2);
                        Console.WriteLine("{0}\t{1}\t{2}",id,name,age);
                    }
                }
                reader.Close();
            }
        }
    }
}
