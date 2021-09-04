using System.Collections.Generic;
using WebCore.ViewModels.Catalog.Common;

namespace WebCore.ViewModels.Catalog.Products
{
    public class GetManageProductPagingRequest: PagingRequest
    {
        public string Keyword { get; set; }
        public List<int> CategoryIds { get; set; }
    }
}
