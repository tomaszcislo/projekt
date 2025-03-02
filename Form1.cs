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
    public partial class Form1 : Form
    {
        static string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
        SqlConnection connect =
            new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\revci\Desktop\ProjektC#\Kurier\Kurier\Kurier\kurier_baza1.mdf;Integrated Security=True;Connect Timeout=30");
        public Form1()
        {
            InitializeComponent();
        }

        private void login_signupBtn_Click(object sender, EventArgs e)
        {
            RegisterForm frm = new RegisterForm();
            frm.Show();
            this.Hide();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            Login data_login = new Login(login_username.Text.Trim(), login_password.Text.Trim());

            if (data_login.Email == ""
                || data_login.Password == "")
            {
                MessageBox.Show("Uzupełnij puste pola"
                   , "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connect.State == ConnectionState.Closed)
                {
                    try
                    {
                        connect.Open();

                        string selectData = "SELECT * FROM users WHERE email = @email " +
                            "AND password = @password";

                        using (SqlCommand cmd = new SqlCommand(selectData, connect))
                        {
                            cmd.Parameters.AddWithValue("@email", data_login.Email.Trim());
                            cmd.Parameters.AddWithValue("@password", data_login.Password.Trim());

                            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show("Udane logowanie",
                                    "Logowanie", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                SqlDataReader reader = cmd.ExecuteReader();

                                string value = "";
                                string emailValue = "";
                                while (reader.Read())
                                {
                                    value = reader.GetValue(0).ToString();
                                    emailValue = reader.GetValue(1).ToString();
                                }

                                if(data_login.Email == "admin")
                                {
                                    Admin admin = new Admin(int.Parse(value), emailValue);
                                    admin.Show();
                                    this.Hide();
                                }
                                else
                                {
                                    MainForm mForm = new MainForm(int.Parse(value), emailValue);
                                    mForm.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Niepoprawny Login/Hasło",
                                    "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
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

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        { 
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }
    
    }
}
