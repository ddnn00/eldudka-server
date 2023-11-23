namespace EldudkaServer.Models.DAL
{
    public class ProductDAL
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public IEnumerable<string> Images { get; set; }
        public IEnumerable<ProductStockDAL> Stock { get; set; }
    }

    public class ProductStockDAL
    {
        public short Amount { get; set; }
        public ShopDAL Shop { get; set; }
    }
}
