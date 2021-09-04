using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Application.Catalog.Products;
using WebCore.ViewModels.Catalog.Products;

namespace WebCore.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IPublicProductService _productService;
        public ProductController(IPublicProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list =await _productService.GetAll();
            return Ok(list);
        }
    }
}
