﻿using Microsoft.AspNetCore.Http;
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
        public string Level { get; set; }
        public IFormFile Image { get; set; }
    }
}