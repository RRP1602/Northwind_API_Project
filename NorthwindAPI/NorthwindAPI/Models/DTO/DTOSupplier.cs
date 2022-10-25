namespace NorthwindAPI.Models.DTO
{
  
        public class DTOSupplier
        {
            public DTOSupplier()
            {
                
            }
            public int SupplierId { get; set; }
            public string CompanyName { get; set; }
            public string? Country { get; set; }
            public int TotalProducts { get; init; }
            
        }
    
}
