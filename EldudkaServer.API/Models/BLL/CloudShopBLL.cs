using Newtonsoft.Json;

namespace EldudkaServer.Models.BLL
{
    public class GetProductsResponse
    {
        [JsonProperty("total")]
        public int? Total { get; set; }

        [JsonProperty("data")]
        public IEnumerable<Product>? Data { get; set; }
    }

    public class Product
    {
        [JsonProperty("uuid")]
        public Guid UUID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = String.Empty;

        [JsonProperty("pic")]
        public IEnumerable<string> Pic { get; set; } = new List<string>();

        [JsonProperty("description")]
        public string Description { get; set; } = String.Empty;

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("stock")]
        public Stock Stock { get; set; } = new Stock();
    }

    public class Stock
    {
        [JsonProperty("5ee27c1b0bd70c2450539856")]
        public Int16 Kulakova { get; set; } = 0;

        [JsonProperty("5ee27c140bd70c7bca3234a7")]
        public Int16 Tukhachevsky { get; set; } = 0;

        [JsonProperty("5be45da00bd70c2cb227bf2b")]
        public Int16 Gallery { get; set; } = 0;

        [JsonProperty("61a13230409cb37e233c2fc8")]
        public Int16 Chocolate { get; set; } = 0;
    }
}
