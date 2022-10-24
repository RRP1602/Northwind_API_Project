namespace NorthwindAPI.Models.DTO
{
    public class DTOOrderDetails
    {
        public DTOOrderDetails()
        {
            Products = new List<DTOProduct>();
        }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public float TotalPrice { get; set; }
        public float TotalDiscount { get; set; }
        public int TotalProducts { get; init; }
        public virtual ICollection<DTOProduct> Products { get; set; }
    }
}
