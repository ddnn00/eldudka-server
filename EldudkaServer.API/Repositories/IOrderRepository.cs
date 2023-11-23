using EldudkaServer.Models.DAL;
using EldudkaServer.Models.DTO;

namespace EldudkaServer.Repositories
{
    public interface IOrderRepository
    {
        Task<int> AddAsync(IEnumerable<ProductIdWithAmountDTO> products);
        Task<IEnumerable<OrderDAL>> GetAllAsync();
    }
}
