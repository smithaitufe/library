using System.Collections.Generic;

namespace Library.Code
{
    public static class Roles {
         private static readonly string[] roles = { "Administrator", "Member", "Librarian", "Webmaster", "Account", "SuperAdministrator" };

        // public static string Admin { get { return roles[0]; } }
        public static string Admin => roles[0];
        public static string Member { get { return roles[1]; }} 
        public static string SuperAdministrator { get { return roles[5]; }} 

        public static IEnumerable<string> All { get { return roles; } }
    }
}