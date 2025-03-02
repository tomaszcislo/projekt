using Kurier.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kurier
{
    public partial class About : Form
    {
        static string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
        private int id_uzytkownika;
        private string email_uzytkownika;

        public About(int id, string email_uzytkownika)
        {
            this.id_uzytkownika = id;
            this.email_uzytkownika = email_uzytkownika;

            InitializeComponent();
        }


        private void data_Load(object sender, EventArgs e)
        {
            set_about();
        }

        private void set_about()
        {

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM ustawienia", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    string telefon = "";
                    string wersja = "";
                    while (reader.Read())
                    {
                        telefon = reader.GetValue(1).ToString();
                        wersja = reader.GetValue(2).ToString();
                    }

                    Settings set = new Settings(telefon, wersja);

                    this.telefon.Text = set.Phone;
                    this.wersja.Text = set.Version;
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (email_uzytkownika == "admin")
            {
                Admin admin = new Admin(id_uzytkownika, email_uzytkownika);
                admin.Show();
                this.Hide();
            }
            else
            {
                history history1 = new history(id_uzytkownika, email_uzytkownika);
                history1.Show();
                this.Hide();
            }
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
    }
}
