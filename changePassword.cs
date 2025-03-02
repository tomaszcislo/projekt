using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Kurier.models;

namespace Kurier
{
    public partial class changePassword : Form
    {
        public changePassword(int id, string email)
        {
            InitializeComponent();
            this.email_uzytkownika = email;
            this.id_uzytkownika = id;
        }

        static string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;
        SqlConnection connect =
            new SqlConnection(connString);
        private int id_uzytkownika;
        private string email_uzytkownika;

        private void button5_Click(object sender, EventArgs e)
        {
            Change_password change_password = new Change_password(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text);

            SqlDataAdapter ASDF = new SqlDataAdapter("SELECT COUNT (*) FROM users WHERE email= '" + change_password.Email + "' AND password='"+ change_password.Password + "'",connect);
            DataTable DS = new DataTable();
            ASDF.Fill(DS);
            errorProvider1.Clear();
            if (DS.Rows[0][0].ToString() == "1")
            {
                if(change_password.New_password == change_password.Confirm_password)
                {
                    SqlDataAdapter cc = new SqlDataAdapter("update users set password='"+ change_password.New_password + "'where email='"+ change_password.Email + "' AND password='"+ change_password.Password + "' ",connect);
                    DataTable DF = new DataTable();
                    cc.Fill(DF);
                    MessageBox.Show("Hasło zostało zmienione","message", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                else
                {
                    errorProvider1.SetError(textBox3, "Hasła nie są takie same");
                    errorProvider1.SetError(textBox3, "Hasła nie są takie same");
                }
            }
            else
            {
                errorProvider1.SetError(textBox1,"incorrect user name");
                errorProvider1.SetError(textBox2, "incorrect password");
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void button1_Click(object sender, EventArgs e)
        {
            MainForm mForm = new MainForm(id_uzytkownika, email_uzytkownika);
            mForm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
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
