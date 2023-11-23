using EldudkaServer.Models.VO;

namespace EldudkaServer.Services
{
    public interface IProductService
    {
        Task<DataWithTotalVO> GetAllWithPaginationAsync(RangeVO range);
        Task<DataWithTotalVO> FilterAsync(IEnumerable<LabelObjectValuesVO> filters);
    }
}
