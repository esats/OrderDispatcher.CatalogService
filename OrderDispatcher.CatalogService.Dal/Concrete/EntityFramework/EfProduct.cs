using OrderDispatcher.CatalogService.Core.EntityFramework;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;

namespace OrderDispatcher.CatalogService.Dal.Concrete.EntityFramework
{
    public class EfProduct : EfEntityRepositoryBase<Product, CatalogServiceDBContext>, IProduct
    {

    }
}