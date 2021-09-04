using System.Collections.Generic;
using System.Threading.Tasks;
using WebCore.ViewModels.Catalog.Common;
using WebCore.ViewModels.Catalog.Products;

namespace WebCore.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PageResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);
        Task<List<ProductViewModel>> GetAll();
    }
}
