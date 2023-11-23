using EldudkaServer.Models.DTO;

namespace EldudkaServer.Models.DAL
{
    public class OrderDAL
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }

        public IEnumerable<ProductIdWithAmountDTO> Products { get; set; }
    }
}
