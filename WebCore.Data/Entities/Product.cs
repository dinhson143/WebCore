using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCore.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public int ViewCount { get; set; }
        public DateTime DateCreated { get; set; }

        // N - N : Category
        public List<ProductInCategory> ProductInCategories { get; set; }

        // 1 - N : OrderDetail
        public List<OrderDetail> OrderDetails { get; set; }

        // N - N : language

        public List<ProductTranslation> ProductTranslations { get; set; }

        // 
        public List<Cart> Carts { get; set; }

    }
}
