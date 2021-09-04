using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Application.Catalog.Products;
using WebCore.ViewModels.Catalog.ProductImages;
using WebCore.ViewModels.Catalog.Products;

namespace WebCore.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IPublicProductService _publicProductService;
        private readonly IManageProductService _manageProductService;

        public ProductsController(IPublicProductService publicProductService, IManageProductService manageProductService)
        {
            _publicProductService = publicProductService;
            _manageProductService = manageProductService;
        }

        // public

        // http://localhost:port/products/pageIndex=?%pageSize=10%categoryId=?
        // From query: tất cả các tham số đều lấy từ from query
        [HttpGet("public-paging/{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetPublicProductPagingRequest request)
        {
            var list = await _publicProductService.GetAllByCategoryId(languageId, request);
            return Ok(list); // status 200
        }

        // manage

        // http://localhost:port/product/{id}
        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var product = await _manageProductService.getById(productId, languageId);
            if (product == null)
            {
                return BadRequest("Can not find product");
            }
            return Ok(product); // status 200
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productId = await _manageProductService.Create(create);
            if (productId == 0)
            {
                return BadRequest();  // status 400
            }

            var product = await _manageProductService.getById(productId, create.LanguageId);

            return Created(nameof(GetById), product); // status 200
            /*return CreatedAtAction(nameof(GetById),new { id= productId}, product);*/
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countAffect = await _manageProductService.Update(request);
            if (countAffect == 0)
            {
                return BadRequest();  // status 400
            }
            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var countAffect = await _manageProductService.Delete(productId);
            if (countAffect == 0)
            {
                return BadRequest();  // status 400
            }
            return Ok();
        }

        [HttpPatch("{productId}/{newPrice}")]   // update một phần của bản ghi => HttpPatch
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSuccess = await _manageProductService.UpdatePrice(productId, newPrice);
            if (isSuccess == false)
            {
                return BadRequest();  // status 400
            }
            return Ok();
        }

        // IMAGES

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _manageProductService.GetImageById(imageId);
            if (image == null)
            {
                return BadRequest("Can not find product");
            }
            return Ok(image); // status 200
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> CreateImage(int productId, [FromForm] ProductImageCreateRequest create)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var imageId = await _manageProductService.AddImage(productId, create);
            if (imageId == 0)
            {
                return BadRequest();  // status 400
            }

            var image = await _manageProductService.GetImageById(imageId);

            /*return Created(nameof(GetById), product);*/ // status 200
            return CreatedAtAction(nameof(GetImageById), new { id = imageId }, image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int productId, int imageId, [FromForm] ProductImageUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var countAffect = await _manageProductService.UpdateImage(imageId, request);
            if (countAffect == 0)
            {
                return BadRequest();  // status 400
            }
            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            var countAffect = await _manageProductService.RemoveImage(imageId);
            if (countAffect == 0)
            {
                return BadRequest();  // status 400
            }
            return Ok();
        }
    }
}