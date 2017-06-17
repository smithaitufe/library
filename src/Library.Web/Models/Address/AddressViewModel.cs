using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.AddressViewModels 
{
    public class AddressViewModel 
    {
        public int AddressId { get; set; }
        public string City { get; set; }
        public string Line { get; set; }
        public int StateId { get; set; }
        public DateTime InsertedAt { get; set; }        
    }
}