using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GuildrAPI.Models
{
    public class GuildrAPIContext : DbContext
    {
        public GuildrAPIContext (DbContextOptions<GuildrAPIContext> options)
            : base(options)
        {
        }

        public DbSet<GuildrAPI.Models.ProfileItem> ProfileItem { get; set; }

        public DbSet<GuildrAPI.Models.PartyItem> PartyItem { get; set; }
    }
}
