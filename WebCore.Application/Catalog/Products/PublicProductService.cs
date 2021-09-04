using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.Data.EF;
using WebCore.ViewModels.Catalog.Common;
using WebCore.ViewModels.Catalog.Products;

namespace WebCore.Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly AppDbContext _context;
        public PublicProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PageResult<ProductViewModel>> GetAllByCategoryId(string languageId,GetPublicProductPagingRequest request)
        {
            // 1. Select Join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, pic };

            // 2. Filter
            if (request.CategoryId.Value > 0 && request.CategoryId.HasValue)
            {
                query = query.Where(x => x.pic.CategoryId == request.CategoryId);
            }

            // Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize)
                .Take(request.pageSize)
                .Select(x => new ProductViewModel()
                {
                    Id = x.p.Id,
                    Price = x.p.Price,
                    OriginalPrice = x.p.OriginalPrice,
                    Stock = x.p.Stock,
                    ViewCount = x.p.ViewCount,
                    DateCreated = x.p.DateCreated,
                    Name = x.pt.Name,
                    Description = x.pt.Description,
                    Details = x.pt.Details,
                    SeoDescription = x.pt.SeoDescription,
                    SeoTitle = x.pt.SeoTitle,
                    SeoAlias = x.pt.SeoAlias,
                    LanguageId = x.pt.LanguageId
                }).ToListAsync();
            // 4 Select Page Result
            var pageResult = new PageResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data
            };

            return pageResult;
        }
    }
}
