using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;

namespace WPFSQLTODO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
         
        private void mybtnsave_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefConn"].ConnectionString;
            int id = Convert.ToInt32(myid.Text);
            int status;
            string title = myttitle.Text;
            if (mystatus.IsChecked == true) { status = 1; }
            else { status = 0; }
                
            string comment = mycomm.Text;
            int mark = Convert.ToInt32(mymark.Text);



            string sqlExpression = string.Format("Insert into TodoItem(id,title,status,comment,mark) Values('{0}','{1}','{2}','{3}',{4}')", id, title, status, comment, mark);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                //SqlDataReader reader = command.ExecuteReader();
                
            }
        }

        private void mybtnshow_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DefConn"].ConnectionString;
            string sqlExpression = "SELECT * FROM TodoItem";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", reader.GetName(0), reader.GetName(1), reader.GetName(2), reader.GetName(3), reader.GetName(4));
                    while (reader.Read())
                    {
                        object id = reader.GetName(0);
                        object title = reader.GetName(1);
                        object status = reader.GetName(2);
                        object comment = reader.GetName(3);
                        object mark = reader.GetName(4);

                        string s = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", id, title, status, comment, mark);
                        myouttxt.Text = s;
                    }
                }
            }
        }
    }
}
