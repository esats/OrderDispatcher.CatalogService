using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderDispatcher.CatalogService.API.Base;
using OrderDispatcher.CatalogService.API.Models;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.API.Controllers
{
    [Route("api/catalog/store")]
    [Produces("application/json")]
    public class StoreController : APIControllerBase
    {
        private readonly IStoreProduct _storeProduct;
        private readonly IProduct _product;

        public StoreController(IStoreProduct storeProduct, IProduct product)
        {
            _storeProduct = storeProduct;
            _product = product;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<Response<HttpStatusCode>> Save([FromForm] IFormFile file)
        {
            Response<HttpStatusCode> response = new Response<HttpStatusCode>();

            if (file == null || file.Length == 0)
            {
                response.Value = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Message = "CSV dosyasi bos.";
                return response;
            }

            try
            {
                using var reader = new StreamReader(file.OpenReadStream());
                var header = await reader.ReadLineAsync();
                if (header == null)
                {
                    response.Value = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.Message = "CSV dosyasi okunamadi.";
                    return response;
                }

                var lineNumber = 1;
                var rows = new List<(string Sku, decimal Price, int Stock, string StoreId)>();
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var parts = line.Split(',', StringSplitOptions.TrimEntries);
                    if (parts.Length < 4)
                    {
                        throw new FormatException($"CSV formatinda hata. Satir: {lineNumber}");
                    }

                    if (!decimal.TryParse(parts[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
                    {
                        throw new FormatException($"Fiyat parse edilemedi. Satir: {lineNumber}");
                    }

                    if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var stock))
                    {
                        throw new FormatException($"Stok parse edilemedi. Satir: {lineNumber}");
                    }

                    rows.Add((parts[0], price, stock, parts[3]));
                }

                if (rows.Count == 0)
                {
                    response.Value = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.Message = "CSV dosyasinda satir bulunamadi.";
                    return response;
                }

                var storeIds = rows.Select(x => x.StoreId).Distinct().ToList();
                var skus = rows.Select(x => x.Sku).Distinct().ToList();

                var existing = _storeProduct.GetList(x => storeIds.Contains(x.StoreId) && skus.Contains(x.SKU));
                var existingMap = existing.ToDictionary(x => $"{x.StoreId}:{x.SKU}", x => x);

                var toInsert = new List<StoreProduct>();
                var toUpdate = new List<StoreProduct>();

                foreach (var row in rows)
                {
                    var key = $"{row.StoreId}:{row.Sku}";
                    if (existingMap.TryGetValue(key, out var entity))
                    {
                        entity.Price = row.Price;
                        entity.Stock = row.Stock;
                        entity.ModifiedBy = base.GetUser();
                        entity.ModifiedDate = DateTime.Now;
                        toUpdate.Add(entity);
                    }
                    else
                    {
                        toInsert.Add(new StoreProduct
                        {
                            SKU = row.Sku,
                            Price = row.Price,
                            Stock = row.Stock,
                            StoreId = row.StoreId,
                            CreatedBy = base.GetUser(),
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        });
                    }
                }

                if (toInsert.Count > 0)
                {
                    _storeProduct.AddRange(toInsert);
                }

                if (toUpdate.Count > 0)
                {
                    _storeProduct.UpdateRange(toUpdate);
                }
            }
            catch (Exception e)
            {
                response.Value = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Message = e.Message;
                return response;
            }

            response.Value = HttpStatusCode.OK;
            response.IsSuccess = true;
            return response;
        }

        [HttpGet]
        [Route("List")]
        public async Task<List<StoreProductListDto>> List(string storeId)
        {
            List<StoreProductListDto> list = new List<StoreProductListDto>();
            try
            {
                var storeProducts = await _storeProduct.GetListAsync(x => x.IsActive && x.StoreId == storeId);
                if (storeProducts.Count == 0)
                {
                    return list;
                }

                var skuList = storeProducts.Select(x => x.SKU).Distinct().ToList();
                var products = await _product.GetListAsync(x => x.IsActive && skuList.Contains(x.SKU));

                var joined = from storeProduct in storeProducts
                             join product in products on storeProduct.SKU equals product.SKU
                             select new { storeProduct, product };

                foreach (var row in joined)
                {
                    StoreProductListDto model = new StoreProductListDto()
                    {
                        Id = row.product.Id,
                        Name = row.product.Name,
                        Description = row.product.Description,
                        Price = row.storeProduct.Price,
                        Stock = row.storeProduct.Stock,
                        BrandId = row.product.BrandId,
                        CategoryId = row.product.CategoryId,
                        ImageMasterId = row.product.ImageMasterId,
                        SKU = row.product.SKU,
                        Order = 0
                    };
                    list.Add(model);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }
    }
}
