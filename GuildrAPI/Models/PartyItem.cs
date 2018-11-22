using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuildrAPI.Models
{
    public class PartyItem
    {
        public int Id { get; set; }
        public string PartyName { get; set; }
        public string Uploaded { get; set; }
        public string Organizer { get; set; }
        public string MemberOne { get; set; }
        public string MemberTwo { get; set; }
        public string MemberThree { get; set; }
    }
}
