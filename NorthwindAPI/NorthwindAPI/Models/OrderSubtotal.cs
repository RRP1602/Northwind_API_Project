using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NorthwindAPI.Models
{
    [ExcludeFromCodeCoverage]

    public partial class OrderSubtotal
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}
