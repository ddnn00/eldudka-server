using Microsoft.AspNetCore.Mvc;
using EldudkaServer.Models.DTO;
using EldudkaServer.Repositories;

namespace EldudkaServer.Controllers
{
    public class OrderController : ControllerBase
    {
        IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("api/order")]
        public async Task<ActionResult> AddAsync(
            [FromBody] IEnumerable<ProductIdWithAmountDTO> products
        )
        {
            return Ok(await _orderRepository.AddAsync(products));
        }

        [HttpGet]
        [Route("api/order")]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _orderRepository.GetAllAsync());
        }
    }
}
