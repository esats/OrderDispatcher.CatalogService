using OrderDispatcher.CatalogService.Core.DataAccess;
using OrderDispatcher.CatalogService.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.Dal.Abstract
{
    public interface IBrand : IEntityRepository<Brand>
    {
    }
}