using Microsoft.AspNetCore.Mvc;
using EldudkaServer.Repositories;
using EldudkaServer.Models.VO;
using EldudkaServer.Services;

namespace EldudkaServer.Controllers
{
    public class ProductController : ControllerBase
    {
        IProductRepository _productRepository;
        IProductService _productService;

        public ProductController(
            IProductRepository productRepository,
            IProductService productService
        )
        {
            _productRepository = productRepository;
            _productService = productService;
        }

        [HttpGet]
        [Route("api/product/{id}")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return Ok(await _productRepository.GetByIdAsync(id));
        }

        [HttpGet]
        [Route("api/product/all")]
        public async Task<ActionResult> GetAllAsync()
        {
            return Ok(await _productRepository.GetAllAsync());
        }

        [HttpPost]
        [Route("api/product/allWithPagination")]
        public async Task<ActionResult> GetAllWithPaginationAsync([FromBody] RangeVO range)
        {
            return Ok(await _productService.GetAllWithPaginationAsync(range));
        }

        [HttpPost]
        [Route("api/product/filter")]
        public async Task<ActionResult> FilterAsync(
            [FromBody] IEnumerable<LabelObjectValuesVO> filters
        )
        {
            return Ok(await _productService.FilterAsync(filters));
        }
    }
}
