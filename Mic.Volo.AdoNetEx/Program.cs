﻿using System;
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
            // SaveFileToDatabase();
            ReadFileFromDatabase();
        }
        private static void ReadFileFromDatabase()
        {
            List<Image> images = new List<Image>();
            using (SqlConnection connection=new SqlConnection(connectoinString))
            {
                connection.Open();
                string sql = "SELECT * FROM Images";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string filename = reader.GetString(1);
                    string title = reader.GetString(2);
                    byte[] data = (byte[])reader.GetValue(3);
                    Image image = new Image(id, filename, title, data);
                    images.Add(image);
                }
            }
            if(images.Count>0)
            {
                using (System.IO.FileStream fs=new System.IO.FileStream(images[0].FileName,System.IO.FileMode.OpenOrCreate))
                {
                    fs.Write(images[0].Data, 0, images[0].Data.Length);
                    Console.WriteLine("Picture '{0}' saved",images[0].Title);
                }
            }
        }
        private static void SaveFileToDatabase()
        {
            using (SqlConnection connection=new SqlConnection(connectoinString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"INSERT INTO Images VALUES (@FileName,@Title,@ImageData)";
                command.Parameters.Add("@FileName", System.Data.SqlDbType.NVarChar, 50);
                command.Parameters.Add("@Title", System.Data.SqlDbType.NVarChar, 50);
                command.Parameters.Add("@ImageData", System.Data.SqlDbType.Image, 1000000);

                string filename = @"E:\Tyom\Pictures\IMG_1113.JPG";
                string title = "esem";
                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);
                byte[] imageData;
                using (System.IO.FileStream fs=new System.IO.FileStream(filename,System.IO.FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                command.Parameters["@FileName"].Value = shortFileName;
                command.Parameters["@Title"].Value = title;
                command.Parameters["@ImageData"].Value = imageData;

                command.ExecuteNonQuery();
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
