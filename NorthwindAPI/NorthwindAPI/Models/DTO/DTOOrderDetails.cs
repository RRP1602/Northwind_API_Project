namespace NorthwindAPI.Models.DTO
{
    public class DTOOrderDetails
    {
        public DTOOrderDetails()
        {
           
        }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public float TotalPrice { get; set; }
        public float TotalDiscount { get; set; }
        public int TotalProducts { get; init; }
        
    }
}
