using Kurier.models;
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

namespace Kurier
{
    public partial class MainForm : Form
    {
        static string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
        SqlConnection connect = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\revci\Desktop\ProjektC#\Kurier\Kurier\Kurier\kurier_baza1.mdf;Integrated Security=True;Connect Timeout=30");

        public MainForm(int id_uzytkownika, string email_uzytkownika)
        {
            InitializeComponent();
            this.id_uzytkownika = id_uzytkownika;
            this.email_uzytkownika = email_uzytkownika;
        }

        private int id_uzytkownika;
        private string email_uzytkownika;

        private void button3_Click(object sender, EventArgs e)
        {
            Shipping_history shipping = new Shipping_history(id_uzytkownika, imie_odbiorcy.Text, nazwisko_odbiorcy.Text, ulica_odbiorcy.Text, nr_domu_odbiorcy.Text, kod_pocztowy_odbiorcy.Text, miejscowosc_odbiorcy.Text, email_odbiorcy.Text, wymiary.Text, waga.Text);

            if (shipping.Name == ""
                || shipping.Last_name == ""
                || shipping.Street == ""
                || shipping.House_number == ""
                || shipping.Zip_code == ""
                || shipping.Domicile == ""
                || shipping.Email == ""
                || shipping.Dimensions == ""
                || shipping.Weight == "")
            {
                MessageBox.Show("Please fill all blank fields"
                    , "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State != ConnectionState.Open)
                {
                    try
                    {
                        connect.Open();

                        string insertData = "INSERT INTO historia_wysylek " +
                                    "(imie_odbiorcy, nazwisko_odbiorcy, ulica_odbiorcy, nr_domu_odbiorcy, kod_pocztowy_odbiorcy, miejscowosc_odbiorcy, email_odbiorcy, wymiary, waga, id_uzytkownika) " +
                                    "VALUES(@imie_odbiorcy, @nazwisko_odbiorcy, @ulica_odbiorcy, @nr_domu_odbiorcy, @kod_pocztowy_odbiorcy, @miejscowosc_odbiorcy, @email_odbiorcy, @wymiary, @waga, " + id_uzytkownika + ")";

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@imie_odbiorcy", shipping.Name.Trim());
                            cmd.Parameters.AddWithValue("@nazwisko_odbiorcy", shipping.Last_name.Trim());
                            cmd.Parameters.AddWithValue("@ulica_odbiorcy", shipping.Street.Trim());
                            cmd.Parameters.AddWithValue("@nr_domu_odbiorcy", shipping.House_number.Trim());
                            cmd.Parameters.AddWithValue("@kod_pocztowy_odbiorcy", shipping.Zip_code.Trim());
                            cmd.Parameters.AddWithValue("@miejscowosc_odbiorcy", shipping.Domicile.Trim());
                            cmd.Parameters.AddWithValue("@email_odbiorcy", shipping.Email.Trim());
                            cmd.Parameters.AddWithValue("@wymiary", shipping.Dimensions.Trim());
                            cmd.Parameters.AddWithValue("@waga", shipping.Weight.Trim());

                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Paczka została nadana!"
                                , "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }


                        Express_points point_value = new Express_points(id_uzytkownika, 0);
                        using (SqlCommand cmd = new SqlCommand("SELECT * FROM punkty WHERE id_uzytkownika = " + id_uzytkownika, connect))
                        {

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count != 0)
                            {
                                insertData = "UPDATE punkty " +
                                            "SET suma_punktow = @suma_punktow WHERE id_uzytkownika = "+ id_uzytkownika;

                                SqlDataReader reader = cmd.ExecuteReader();

                                string value = "";
                                while (reader.Read())
                                {
                                    value = reader.GetValue(2).ToString();
                                }

                                point_value.Points = int.Parse(value);

                                reader.Close();
                            }
                            else
                            {
                                insertData = "INSERT INTO punkty " +
                                    "(id_uzytkownika, suma_punktow) " +
                                    "VALUES(@id_uzytkownika, @suma_punktow)";
                            }

                            
                        }

                        using (SqlCommand cmd = new SqlCommand(insertData, connect))
                        {
                            cmd.Parameters.AddWithValue("@id_uzytkownika", point_value.User_id.ToString().Trim());
                            cmd.Parameters.AddWithValue("@suma_punktow", (point_value.Points+5).ToString().Trim());

                            cmd.ExecuteNonQuery();
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

        private void data_Load(object sender, EventArgs e)
        {
            set_price();
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

                    this.mala_paczka.Text = price.Small_pack;
                    this.srednia_paczka.Text = price.Medium_pack;
                    this.duza_paczka.Text = price.Large_pack;
                }

            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(email_uzytkownika == "admin")
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
            Contact contact1 = new Contact(id_uzytkownika, email_uzytkownika);
            contact1.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            About about1 = new About(id_uzytkownika, email_uzytkownika);
            about1.Show();
            this.Hide();
        }
    }
}
