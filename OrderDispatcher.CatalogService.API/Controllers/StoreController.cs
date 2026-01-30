using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderDispatcher.CatalogService.API.Base;

namespace OrderDispatcher.CatalogService.API.Controllers
{
    [Route("api/catalog/store")]
    [Produces("application/json")]
    public class StoreController : APIControllerBase
    {
    }
}
