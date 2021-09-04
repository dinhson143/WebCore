using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCore.ViewModels.Catalog.Common;
using WebCore.ViewModels.Catalog.Products;

namespace WebCore.Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create(ProductCreateRequest create);
        Task<int> Update(ProductUpdateRequest request);
        Task<int> Delete(int productId);

        Task<bool> UpdatePrice(int productId, decimal newPrice);
        Task<bool> UpdateStock(int productId, int addQuantity);

        Task AddViewCount(int productId);

        Task<PageResult <ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request);
        Task<int> addImage(int productId, List<IFormFile> files);
        Task<int> removeImage(int imageId);
        Task<int> UpdateImage(int imageId, string caption,bool isDefault);
        Task<List<ProductImageViewModel>> GetListImage(int productId);
    }
}
