using OrderDispatcher.CatalogService.Core.EntityFramework;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;
using System.Collections.Generic;

namespace OrderDispatcher.CatalogService.Dal.Concrete.EntityFramework
{
    public class EfStoreProduct : EfEntityRepositoryBase<StoreProduct, CatalogServiceDBContext>, IStoreProduct
    {
        public EfStoreProduct(CatalogServiceDBContext context) : base(context) { }

        public void AddRange(List<StoreProduct> entities)
        {
            _context.Set<StoreProduct>().AddRange(entities);
            _context.SaveChanges();
        }

        public void UpdateRange(List<StoreProduct> entities)
        {
            _context.Set<StoreProduct>().UpdateRange(entities);
            _context.SaveChanges();
        }
    }
}
