using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Price_list
    {
        public int Id { get; set; }
        public string Small_pack { get; set; }
        public string Medium_pack { get; set; }
        public string Large_pack { get; set; }

        public Price_list(string small_pack, string medium_pack, string large_pack) 
        { 
            Small_pack = small_pack;
            Medium_pack = medium_pack;
            Large_pack = large_pack;
        }

    }
}
