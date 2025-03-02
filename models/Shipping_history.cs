using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Shipping_history
    {
        public int Id { get; set; }
        public int User_id { get; set; }

        public string Name { get; set; }
        public string Last_name { get; set; }
        public string Street { get; set; }
        public string House_number { get; set; }
        public string Zip_code { get; set; }
        public string Domicile { get; set; }
        public string Email { get; set; }

        public string Dimensions { get; set; }
        public string Weight { get; set; }

        public Shipping_history(int user_id, string name, string last_name, string street, string house_number, string zip_code, string domicile, string email, string dimensions, string weight)
        {
            User_id = user_id;
            Name = name;
            Last_name = last_name;
            Street = street;
            House_number = house_number;
            Zip_code = zip_code;
            Domicile = domicile;
            Email = email;
            Dimensions = dimensions;
            Weight = weight;
        }
    }
}
