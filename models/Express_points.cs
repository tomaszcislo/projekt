using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Express_points
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Points {  get; set; }

        public Express_points(int user_id, int points)
        {
            User_id = user_id;
            Points = points;
        }
    }
}
