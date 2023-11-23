using EldudkaServer.Models.DAL;
using EldudkaServer.Models.DTO;

namespace EldudkaServer.Repositories
{
    public interface IProductRepository
    {
        Task<ProductDAL> GetByIdAsync(Guid id);
        Task<IEnumerable<ProductDAL>> GetAllAsync();
        Task<int> DeleteAllAsync();
        Task<int> AddRangeAsync(IEnumerable<ProductForCreatingDTO> products);
    }
}
