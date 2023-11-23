namespace EldudkaServer.Models.DTO
{
    public class ProductForCreatingDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public IEnumerable<ProductImageForCreatingDTO> Images { get; set; }
        public IEnumerable<ProductStockForCreatingDTO> Stock { get; set; }
    }

    public class ProductImageForCreatingDTO
    {
        public Guid? ProductId { get; set; }
        public string Href { get; set; }
    }

    public class ProductStockForCreatingDTO
    {
        public Guid? ProductId { get; set; }
        public Int16 ShopId { get; set; }
        public Int16 Amount { get; set; }
    }
}
