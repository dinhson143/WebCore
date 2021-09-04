using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebCore.Application.Common;
using WebCore.Data.EF;
using WebCore.Data.Entities;
using WebCore.Utilities.Exceptions;
using WebCore.ViewModels.Catalog.Common;
using WebCore.ViewModels.Catalog.Products;

namespace WebCore.Application.Catalog.Products
{
    class ManageProductService : IManageProductService
    {
        private readonly AppDbContext _context;
        private readonly IStorageService _storageService;
        public ManageProductService(AppDbContext context, IStorageService storageService)
        {
            context = _context;
            _storageService = storageService;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            product.ViewCount += 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest create)
        {
           var product = new Product()
            {
                Price = create.Price,
                OriginalPrice = create.OriginalPrice,
                Stock = create.Stock,
                ViewCount =0,
                DateCreated = DateTime.Now,
                ProductInCategories = new List<ProductInCategory>()
                {

                },
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name = create.Name,
                        Description = create.Description,
                        Details = create.Details,
                        SeoDescription = create.SeoDescription,
                        SeoAlias = create.SeoAlias,
                        SeoTitle = create.SeoTitle,
                        LanguageId = create.LanguageId
                    }
                }
            };
            //Save image
            if (create.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption ="Thumbnail "+ create.Name,
                        DateCreated = DateTime.Now,
                        FileSize = create.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(create.ThumbnailImage),
                        IsDefault = true,
                        SortOrder = 1
                    }
                };
            }


            await _context.Products.AddAsync(product);
            return await _context.SaveChangesAsync();

        }

        public async Task<int> Delete(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebCoreException($"Can not find a Product: {productId}");
          

            var images =  _context.ProductImages.Where(x=>x.ProductId==productId);
            foreach(var item in images)
            {
               await _storageService.DeleteFileAsync(item.ImagePath);
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranlation =  await  _context.ProductTranslations.FirstOrDefaultAsync(x=>x.ProductId== request.Id && 
            x.LanguageId == request.LanguageId);
            if (product == null || productTranlation==null) throw new WebCoreException($"Can not find a Product: {request.Id}");

            productTranlation.Name = request.Name;
            productTranlation.SeoAlias = request.SeoAlias;
            productTranlation.SeoDescription = request.SeoDescription;
            productTranlation.SeoTitle = request.SeoTitle;
            productTranlation.Description = request.Description;
            productTranlation.Details = request.Details;


            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnail = await _context.ProductImages.FirstOrDefaultAsync(x => x.IsDefault == true && x.ProductId == request.Id);
                if (thumbnail != null)
                {
                    thumbnail.FileSize = request.ThumbnailImage.Length;
                    thumbnail.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnail);
                }               
            }

            return await _context.SaveChangesAsync();

        }


        public async Task<PageResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            // 1.Select join
            var query = from p in _context.Products
                        join pt in _context.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _context.ProductInCategories on p.Id equals pic.ProductId
                        join c in _context.Categories on pic.CategoryId equals c.Id
                        select new { p,pt,pic};
            // 2. Filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count>0)
            {
                query = query.Where(x=>request.CategoryIds.Contains(x.pic.CategoryId));
            }
            // 3 .Paging
            int totalRow = await query.CountAsync();
            var data = await query.Skip((request.pageIndex - 1) * request.pageSize)
                .Take(request.pageSize)
                .Select(x => new ProductViewModel() {
                        Id = x.p.Id,
                        Price = x.p.Price,
                        OriginalPrice = x.p.OriginalPrice,
                        Stock = x.p.Stock,
                        ViewCount= x.p.ViewCount,
                        DateCreated = x.p.DateCreated,
                        Name  = x.pt.Name,
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

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebCoreException($"Can not find a Product: {productId}");
            product.Price = newPrice;
            return await _context.SaveChangesAsync()>0; // >0 : true
        }

        public async Task<bool> UpdateStock(int productId, int addQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebCoreException($"Can not find a Product: {productId}");
            product.Stock += addQuantity;
            return await _context.SaveChangesAsync() > 0; // >0 : true
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            //return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
            return fileName;
        }

        public async Task<int> addImage(int productId, List<IFormFile> files)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebCoreException($"Can not find a Product: {productId}");

            var images = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();

            if (files.Count > 0)
            {
                foreach (var item in files)
                {
                    // Save File
                    var image =new ProductImage()
                    {
                        Caption = "Thumbnail " + item.Name,
                        DateCreated = DateTime.Now,
                        FileSize = item.Length,
                        ImagePath = await this.SaveFile(item),
                        IsDefault = true,
                        SortOrder = 1
                    };
                    images.Add(image);
                }
            }
            product.ProductImages = images;

            _context.Products.Update(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> removeImage(int imageId)
        {
            var image =await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (image == null) throw new WebCoreException($"Can not find a Image: {imageId}");

            _context.ProductImages.Remove(image);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateImage(int imageId, string caption, bool isDefault)
        {
            var image = await _context.ProductImages.FirstOrDefaultAsync(x => x.Id == imageId);
            if (image == null) throw new WebCoreException($"Can not find a Image: {imageId}");

            image.Caption = caption;
            image.IsDefault = isDefault;

            _context.ProductImages.Update(image);
            return await _context.SaveChangesAsync();

        }

        public async Task<List<ProductImageViewModel>> GetListImage(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new WebCoreException($"Can not find a Product: {productId}");

            var images = await _context.ProductImages.Where(x => x.ProductId == productId).ToListAsync();

            List<ProductImageViewModel> list = new List<ProductImageViewModel>();

            foreach (var item in images)
            {
                var x = new ProductImageViewModel()
                {
                    Id = item.Id,
                    FilePath = item.ImagePath,
                    FileSize = item.ImagePath.Length,
                    isDefault = item.IsDefault
                 };
                list.Add(x);
            }

            return list;
        }
    }
}
