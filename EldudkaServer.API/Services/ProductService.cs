using EldudkaServer.Models.VO;
using EldudkaServer.Repositories;
using System.Linq;
using System.Text.Json;

namespace EldudkaServer.Services
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<DataWithTotalVO> GetAllWithPaginationAsync(RangeVO range)
        {
            var products = await _productRepository.GetAllAsync();

            return new DataWithTotalVO()
            {
                Data = products.Skip(range.From ?? 0).Take(range.To ?? 50),
                Total = products.Count()
            };
        }

        public async Task<DataWithTotalVO> FilterAsync(IEnumerable<LabelObjectValuesVO> filters)
        {
            var products = await _productRepository.GetAllAsync();
            var total = products.Count();

            if (filters.Any())
            {
                foreach (var filter in filters)
                {
                    switch (filter.Label)
                    {
                        case "id":
                            products = products.Where(
                                p => filter.Values.Any(a => a.ToString().Contains(p.Id.ToString()))
                            );
                            break;
                        case "name":
                            products = products.Where(
                                p =>
                                    filter.Values.Any(
                                        a => p.Name.ToLower().Contains(a.ToString().ToLower())
                                    )
                            );
                            break;
                        case "range":
                            var fromTo = filter.Values.ToList();
                            var from = ((JsonElement)fromTo[0]).GetInt32();
                            var to = ((JsonElement)fromTo[1]).GetInt32();

                            products = products
                                .Skip(from)
                                .Take(to - from);
                            break;
                    }
                }
            }

            return new DataWithTotalVO() { Data = products, Total = total };
        }
    }
}
