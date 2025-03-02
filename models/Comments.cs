using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Comments
    {
        public int Id { get; set; }
        public int Id_uzytkownika { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public Comments(int id_uzytkownika, string email, string comment)
        {
            Email = email;
            Comment = comment;
            Id_uzytkownika = id_uzytkownika;
        }
    }
}
