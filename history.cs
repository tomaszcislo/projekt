using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Kurier.models;

namespace Kurier
{
    public partial class history : Form
    {
        static string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\revci\Desktop\ProjektC#\Kurier\Kurier\Kurier\kurier_baza1.mdf;Integrated Security=True;Connect Timeout=30");

        public history(int id_uzytkownika, string email_uzytkownika)
        {
            InitializeComponent();
            this.id_uzytkownika = id_uzytkownika;
            this.email_uzytkownika = email_uzytkownika;
        }

        private int id_uzytkownika;
        private string email_uzytkownika;

        private void history_Load(object sender, EventArgs e)
        {
            historyListdataGridView.DataSource = GetHistoryList();
            get_points();
        }

        private int tableRow = 0;
        private DataTable GetHistoryList()
        {
            DataTable dtHistory = new DataTable();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM historia_wysylek WHERE id_uzytkownika = " + id_uzytkownika, con))
                {
                    con.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    tableRow = table.Rows.Count;

                    SqlDataReader reader = cmd.ExecuteReader();

                    dtHistory.Load(reader);
                }
                
            }

            return dtHistory;
        }

        private void get_points()
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM punkty WHERE id_uzytkownika = " + id_uzytkownika, con))
                {
                    con.Open();

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if(tableRow != 0)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        string value = ""; ;
                        while (reader.Read())
                        {
                            value = reader.GetValue(2).ToString();
                        }

                        Express_points point = new Express_points(id_uzytkownika, int.Parse(value));

                        punkty.Text = point.Points.ToString();
                    }

                }

            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mForm = new MainForm(id_uzytkownika, email_uzytkownika);
            mForm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            changePassword changePassword1 = new changePassword(id_uzytkownika, email_uzytkownika);
            changePassword1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Contact contact1 = new Contact(id_uzytkownika, email_uzytkownika);
            contact1.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            About about1 = new About(id_uzytkownika, email_uzytkownika);
            about1.Show();
            this.Hide();
        }
    }
}
