using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindAPI.Models
{
    [ExcludeFromCodeCoverage]

    public partial class CustomerAndSuppliersByCity
    {
        public string? City { get; set; }
        public string CompanyName { get; set; } = null!;
        public string? ContactName { get; set; }
        public string Relationship { get; set; } = null!;
    }
}
