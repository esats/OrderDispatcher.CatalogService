using OrderDispatcher.CatalogService.Core.EntityFramework;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;
using System.Collections.Generic;

namespace OrderDispatcher.CatalogService.Dal.Concrete.EntityFramework
{
    public class EfStoreProduct : EfEntityRepositoryBase<StoreProduct, CatalogServiceDBContext>, IStoreProduct
    {
        public void AddRange(List<StoreProduct> entities)
        {
            using var context = new CatalogServiceDBContext();
            context.Set<StoreProduct>().AddRange(entities);
            context.SaveChanges();
        }

        public void UpdateRange(List<StoreProduct> entities)
        {
            using var context = new CatalogServiceDBContext();
            context.Set<StoreProduct>().UpdateRange(entities);
            context.SaveChanges();
        }
    }
}
