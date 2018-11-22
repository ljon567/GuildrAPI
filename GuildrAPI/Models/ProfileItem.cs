using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuildrAPI.Models
{
    public class ProfileItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Class { get; set; }
        public string Uploaded { get; set; }
        public string Password { get; set; }
        public int Level { get; set; }
    }
}
