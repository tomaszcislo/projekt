using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurier.models
{
    public class Settings
    {
        public int Id { get; set; }
        public string Phone {  get; set; }
        public string Version { get; set; }
        public Settings(string phone, string version)
        {
            Phone = phone;
            Version = version;
        }
    }
}
