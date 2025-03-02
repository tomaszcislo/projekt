using Kurier.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kurier
{
    public partial class Admin : Form
    {
        public Admin(int id, string email)
        {
            id_uzytkownika = id;
            email_uzytkownika = email;
            InitializeComponent();
        }

        private int id_uzytkownika;
        private string email_uzytkownika;

        string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

        private void adminListdataGridView_Load(object sender, EventArgs e)
        {
            adminListdataGridView.DataSource = GetHistoryList();
            set_price();
            set_settings();
        }
        private DataTable GetHistoryList()
        {
            DataTable dtComments = new DataTable();

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM historia_wysylek", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    dtComments.Load(reader);
                }

            }
            return dtComments;
        }

        private void set_price()
        {

            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM cennik", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    string mala_paczka = "";
                    string srednia_paczka = "";
                    string duza_paczka = "";
                    while (reader.Read())
                    {
                        mala_paczka = reader.GetValue(1).ToString();
                        srednia_paczka = reader.GetValue(2).ToString();
                        duza_paczka = reader.GetValue(3).ToString();
                    }

                    Price_list price = new Price_list(mala_paczka, srednia_paczka, duza_paczka);

                    zmiana_ceny_mala.Text = price.Small_pack;
                    zmiana_ceny_srednia.Text = price.Medium_pack;
                    zmiana_ceny_duza.Text = price.Large_pack;
                }

            }
        }

        private void set_settings()
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 * FROM ustawienia", con))
                {
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    string tel = "";
                    string version = "";
                    while (reader.Read())
                    {
                        tel = reader.GetValue(1).ToString();
                        version = reader.GetValue(2).ToString();
                    }

                    Settings set = new Settings(tel, version);

                    zmiana_numeru.Text = set.Phone;
                    textBox1.Text = set.Version;
                }

            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Price_list price = new Price_list(zmiana_ceny_mala.Text, zmiana_ceny_srednia.Text, zmiana_ceny_duza.Text);

            SqlConnection connect = new SqlConnection(connString);

            if (price.Small_pack == "" || price.Medium_pack == "" || price.Large_pack == "")
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
                        string selectData = "SELECT * FROM cennik";
                        string insertData;

                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                insertData = "UPDATE cennik " +
                                            "SET mala_paczka = @mala_paczka, srednia_paczka = @srednia_paczka, duza_paczka = @duza_paczka";
                            }
                            else
                            {
                                insertData = "INSERT INTO cennik " +
                                            "(mala_paczka, srednia_paczka, duza_paczka) " +
                                            "VALUES(@mala_paczka, @srednia_paczka, @duza_paczka)";
                            }
                            Console.WriteLine(insertData);
                        }

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@mala_paczka", price.Small_pack);
                            cmd.Parameters.AddWithValue("@srednia_paczka", price.Medium_pack);
                            cmd.Parameters.AddWithValue("@duza_paczka", price.Large_pack);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Cennik dodany!"
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
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Settings set = new Settings(zmiana_numeru.Text, textBox1.Text);

            SqlConnection connect = new SqlConnection(connString);
            if (set.Phone == "" || set.Version == "")
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
                        string selectData = "SELECT * FROM ustawienia";
                        string insertData;

                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                insertData = "UPDATE ustawienia " +
                                            "SET telefon = @telefon, wersja = @wersja";
                            }
                            else
                            {
                                insertData = "INSERT INTO ustawienia " +
                                            "(telefon, wersja) " +
                                            "VALUES(@telefon, @wersja)";
                            }
                            Console.WriteLine(insertData);
                        }

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@telefon", set.Phone);
                            cmd.Parameters.AddWithValue("@wersja", set.Version);

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Ustawienia zmienione!"
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
            About information = new About(id_uzytkownika, email_uzytkownika);
            information.Show();
            this.Hide();
        }
    }
}
