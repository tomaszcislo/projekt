using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Date_register { get; set; }

        public Users(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
