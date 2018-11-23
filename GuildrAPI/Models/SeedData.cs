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
                //DB has been seeded already
                if (context.ProfileItem.Count() > 0)
                {
                    return;   
                }

                context.ProfileItem.AddRange(
                    new ProfileItem
                    {
                        Name = "Patrick the Star",
                        Url = "https://i.kym-cdn.com/photos/images/original/001/371/723/be6.jpg",
                        Class = "Mage",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Level = 420,
                        Password = "password"
                    }


                );
                context.SaveChanges();

                //DB has been seeded already
                if (context.PartyItem.Count() > 0)
                {
                    return;
                }

                context.PartyItem.AddRange(
                    new PartyItem
                    {
                        PartyName = "Bikini Bottom Band",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Organizer = "Patrick the Star",
                        MemberOne = "Spongebob the Squarepants",
                        MemberTwo = "Squidward the Tentacles",
                        MemberThree = "Sandy the Cheeks",
                        Password = "password"
                    }


                );
                context.SaveChanges();
            }
        }
    }
}
