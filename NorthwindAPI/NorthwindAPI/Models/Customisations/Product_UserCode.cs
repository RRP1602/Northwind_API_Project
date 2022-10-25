using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NorthwindAPI.Models
{
    public partial class Product
    {
        public override bool Equals(object? obj)
        {
            var p = obj as Product;

            return this.ProductId == p.ProductId &&
                   this.ProductName == p.ProductName &&
                   this.UnitPrice == p.UnitPrice &&
                   this.QuantityPerUnit == p.QuantityPerUnit &&
                   this.UnitsInStock == p.UnitsInStock &&
                   this.UnitsOnOrder == p.UnitsOnOrder &&
                   this.CategoryId == p.CategoryId &&
                   this.SupplierId == p.SupplierId &&
                   this.Discontinued == p.Discontinued &&
                   this.ReorderLevel == p.ReorderLevel;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
