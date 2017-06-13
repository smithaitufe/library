using System.Collections.Generic;

namespace Library.Core.Models
{
    public class Location: BaseEntity {
        public string Name { get; set; }
        public string Code { get; set; }

        public ICollection<VariantLocation> VariantLocations { get; set; }
    }
}