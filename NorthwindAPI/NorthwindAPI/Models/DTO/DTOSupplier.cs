namespace NorthwindAPI.Models.DTO
{
  
        public class DTOSupplier
        {
            public DTOSupplier()
            {
                Products = new List<DTOProduct>();
            }
            public int SupplierId { get; set; }
            public string CompanyName { get; set; }
            public string? ContactName { get; set; }
            public string? ContactTitle { get; set; }
            public string? Country { get; set; }
            public int TotalProducts { get; init; }
            public virtual ICollection<DTOProduct> Products { get; set; }
        }
    
}
