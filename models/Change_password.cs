using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Change_password : Users
    {
        public string New_password { get; set; }
        public string Confirm_password { get; set; }

        public Change_password(string email, string password, string new_password, string confirm_password) : base(email, password)
        {
            Email = email;
            Password = password;
            New_password = new_password;
            Confirm_password = confirm_password;
        }
    }
}
