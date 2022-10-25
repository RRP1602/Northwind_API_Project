namespace NorthwindAPI.Models.DTO
{
    public class DTOProduct
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? UnitPrice { get; set; }
        public DTOSupplier Supplier { get; set; }
        public DTOCategory Category { get; set; }
    }
}
