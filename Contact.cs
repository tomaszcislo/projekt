using Kurier.models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Kurier
{
    public partial class Contact : Form
    {
        string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
        private int id_uzytkownika;
        private string email_uzytkownika;

        public Contact(int id, string email)
        {
            email_uzytkownika = email;
            id_uzytkownika = id;
            InitializeComponent();
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

        private void button5_Click(object sender, EventArgs e)
        {
            About about1 = new About(id_uzytkownika, email_uzytkownika);
            about1.Show();
            this.Hide();
        }

        private void dataGridView1_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = get_comments();
        }
        private DataTable get_comments()
        {
            DataTable dtComments = new DataTable();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT email, komentarz FROM komentarze", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    dtComments.Load(reader);
                }

            }
            return dtComments;
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            Comments nowy_komentarz = new Comments(id_uzytkownika, email_uzytkownika, labelKomentarz.Text.Trim());

            SqlConnection connect = new SqlConnection(connString);

            if (nowy_komentarz.Comment == "")
            {
                MessageBox.Show("Uzupełnij puste pola"
                   , "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();
                        string insertData = "INSERT INTO komentarze " +
                                            "(id_uzytkownika, email, komentarz) " +
                                            "VALUES(@id_uzytkownika, @email, @komentarz)";

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@id_uzytkownika", nowy_komentarz.Id_uzytkownika);
                            cmd.Parameters.AddWithValue("@email", nowy_komentarz.Email);
                            cmd.Parameters.AddWithValue("@komentarz", nowy_komentarz.Comment);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Komentarz dodany!"
                                , "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex
                      , "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }

            dataGridView1.DataSource = get_comments();
            dataGridView1.Update();
            dataGridView1.Refresh();

        }
    }
}
