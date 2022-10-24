using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindAPI.Models
{
    [ExcludeFromCodeCoverage]

    public partial class ProductsAboveAveragePrice
    {
        public string ProductName { get; set; } = null!;
        public decimal? UnitPrice { get; set; }
    }
}
