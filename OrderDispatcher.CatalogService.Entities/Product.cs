using OrderDispatcher.CatalogService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.Entities
{
    public class Product : EntityBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int ImageMasterId { get; set; } = 0;
    }
}
