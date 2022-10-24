using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindAPI.Models
{
    [ExcludeFromCodeCoverage]

    public partial class Region
    {
        public Region()
        {
            Territories = new HashSet<Territory>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; } = null!;

        public virtual ICollection<Territory> Territories { get; set; }
    }
}
