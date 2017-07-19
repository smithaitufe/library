using System;
using System.Collections.Generic;
using Library.Core.Models;

namespace Library.Web.Models.AccountViewModels
{
    public class SignupViewModel
    {        
        public int UserId { get; set; }
        public string LibraryNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime InsertedAt { get; set; }
        public bool Approved { get; set; }
        public bool Checked { get; set; }
        public string FullName => LastName + " " + FirstName;    

        
    }
}