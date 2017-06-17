using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Code
{
    public static class Users
    {
        public static User[] users = {
                    new User { UserName = "08138238095", PhoneNumber = "08138238095", FirstName ="Smith", LastName = "Samuel", Email = "08138238095@librille.com", Approved = true },
                    new User { UserName = "08064028176", PhoneNumber = "08064028176", FirstName ="Charles", LastName = "Omordia", Email = "c.omordia@schoolville.com", Approved = true },
                    new User { UserName = "08037862905", PhoneNumber = "08037862905", FirstName ="Patrick", LastName = "Acha", Email = "p.acha@schoolville.com" },
                    new User { UserName = "08136754265", PhoneNumber = "08136754265", FirstName ="Isioma", LastName = "Omordia", Email = "i.omordia@schoolville.com" },
                    new User { UserName = "08053094604", PhoneNumber = "08053094604", FirstName ="Felix", LastName = "Ateli", Email = "fsunny73@gmail.com" },

                };
        public static IEnumerable<User> All
        {
            get { return users; }
        }
    }
}