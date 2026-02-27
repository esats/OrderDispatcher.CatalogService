using OrderDispatcher.CatalogService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderDispatcher.CatalogService.Entities
{
    public class Category : EntityBase<int> 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public int Order { get; set; }
    }
}
