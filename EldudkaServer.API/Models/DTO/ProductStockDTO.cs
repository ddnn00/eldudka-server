namespace EldudkaServer.Models.DTO
{
    public class ProductStockDTO
    {
        public Guid ProductId { get; set; }
        public short ShopId { get; set; }
        public short Amount { get; set; }
    }
}
