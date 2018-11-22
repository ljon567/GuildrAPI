using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuildrAPI.Models
{
    public class ProfileImageItem
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public int Level { get; set; }
        public string Password { get; set; }
        public IFormFile Image { get; set; }
    }
}