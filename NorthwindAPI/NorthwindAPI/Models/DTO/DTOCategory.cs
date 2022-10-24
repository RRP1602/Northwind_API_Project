namespace NorthwindAPI.Models.DTO
{
    public class DTOCategory
    {
        public DTOCategory()
        {
            Products = new HashSet<DTOProduct>();
        }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? CategoryDescription { get; set; }
        public int TotalProducts { get; init; }
        public virtual ICollection<DTOProduct> Products { get; set; }
    }
}
