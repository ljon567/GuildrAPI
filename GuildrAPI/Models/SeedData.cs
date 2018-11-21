using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuildrAPI.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GuildrAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<GuildrAPIContext>>()))
            {
                // Look for any movies.
                if (context.ProfileItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.ProfileItem.AddRange(
                    new ProfileItem
                    {
                        Name = "Patrick the Star",
                        Url = "https://i.kym-cdn.com/photos/images/original/001/371/723/be6.jpg",
                        Class = "Mage",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Level = "420"
                    }


                );
                context.SaveChanges();
            }
        }
    }
}
