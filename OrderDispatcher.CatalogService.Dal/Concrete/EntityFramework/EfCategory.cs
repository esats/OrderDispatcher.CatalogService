using Microsoft.Data.SqlClient;
using OrderDispatcher.CatalogService.Core.EntityFramework;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.Dal.Concrete.EntityFramework
{
    public class EfCategory : EfEntityRepositoryBase<Category, CatalogServiceDBContext>, ICategory
    {

    }
}