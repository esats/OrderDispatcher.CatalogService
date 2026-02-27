using OrderDispatcher.CatalogService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.Entities
{
    public class StoreProduct : EntityBase<int>
    {
        public string SKU { get; set; }
        public string StoreId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
