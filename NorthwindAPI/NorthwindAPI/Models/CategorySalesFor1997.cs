using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindAPI.Models
{
    [ExcludeFromCodeCoverage]

    public partial class CategorySalesFor1997
    {
        public string CategoryName { get; set; } = null!;
        public decimal? CategorySales { get; set; }
    }
}
