using EldudkaServer.Models.BLL;

namespace EldudkaServer.Services
{
    public interface ICloudShopService
    {
        Task<GetProductsResponse> GetProducts();
    }
}
