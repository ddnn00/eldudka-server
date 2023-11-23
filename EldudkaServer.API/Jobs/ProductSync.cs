using EldudkaServer.Models.DTO;
using EldudkaServer.Repositories;
using EldudkaServer.Services;

namespace EldudkaServer.Jobs
{
    public class ProductSync
    {
        ICloudShopService _cloudShopService;
        IProductRepository _productRepository;

        public ProductSync(ICloudShopService cloudShopService, IProductRepository productRepository)
        {
            _cloudShopService = cloudShopService;
            _productRepository = productRepository;
        }

        public async Task Start()
        {
            var csProducts = await _cloudShopService.GetProducts();

            if (csProducts.Data.Any())
            {
                var deletedProducts = await _productRepository.DeleteAllAsync();
                var insertedProducts = await _productRepository.AddRangeAsync(
                    csProducts.Data.Select(
                        p =>
                            new ProductForCreatingDTO()
                            {
                                Id = p.UUID,
                                Description = p.Description,
                                Name = p.Name,
                                Price = p.Price,
                                Images = p.Pic.Select(
                                    p => new ProductImageForCreatingDTO() { Href = p }
                                ),
                                Stock = new List<ProductStockForCreatingDTO>()
                                {
                                    new ProductStockForCreatingDTO()
                                    {
                                        ShopId = 1,
                                        Amount = p.Stock.Tukhachevsky
                                    },
                                    new ProductStockForCreatingDTO()
                                    {
                                        ShopId = 2,
                                        Amount = p.Stock.Gallery
                                    },
                                    new ProductStockForCreatingDTO()
                                    {
                                        ShopId = 3,
                                        Amount = p.Stock.Chocolate
                                    },
                                    new ProductStockForCreatingDTO()
                                    {
                                        ShopId = 4,
                                        Amount = p.Stock.Kulakova
                                    }
                                }
                            }
                    )
                );
            }
        }
    }
}
