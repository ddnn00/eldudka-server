using EldudkaServer.Models.BLL;
using Newtonsoft.Json;

namespace EldudkaServer.Services
{
    public class CloudShopService : ICloudShopService
    {
        private readonly string _authToken;

        public CloudShopService(IConfiguration configuration)
        {
            _authToken = configuration.GetConnectionString("cloudShopAuthToken");
        }

        public async Task<GetProductsResponse> GetProducts()
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Cookie", _authToken);

            string response = await httpClient.GetStringAsync(
                "https://web.cloudshop.ru/proxy/?path=%2Fdata%2F5be45da00bd70c2cb227bf2a%2Fcatalog&api=v3&timezone=10800"
            );

            return JsonConvert.DeserializeObject<GetProductsResponse>(response);
        }
    }
}
