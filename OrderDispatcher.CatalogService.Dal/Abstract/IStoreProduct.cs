using OrderDispatcher.CatalogService.Core.DataAccess;
using OrderDispatcher.CatalogService.Entities;
using System.Collections.Generic;

namespace OrderDispatcher.CatalogService.Dal.Abstract
{
    public interface IStoreProduct : IEntityRepository<StoreProduct>
    {
        void AddRange(List<StoreProduct> entities);
        void UpdateRange(List<StoreProduct> entities);
    }
}
