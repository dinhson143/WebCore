using WebCore.ViewModels.Catalog.Common;

namespace WebCore.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequest
    {
        public int? CategoryId { get; set; }
    }
}
