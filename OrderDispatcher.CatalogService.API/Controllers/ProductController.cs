using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderDispatcher.CatalogService.API.Base;
using OrderDispatcher.CatalogService.API.Models;
using OrderDispatcher.CatalogService.Core.Entities;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.API.Controllers
{
    [Route("api/catalog/product")]
    [Produces("application/json")]
    public class ProductController : APIControllerBase
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }

        [HttpPost]
        [Route("Save")]
        public Response<HttpStatusCode> Save([FromBody] ProductDto dto)
        {
            Response<HttpStatusCode> response = new Response<HttpStatusCode>();
            try
            {
                if (dto.Id != 0)
                {
                    Product entity = _product.GetT(x => x.Id == dto.Id);
                    entity.Name = dto.Name;
                    entity.Description = dto.Description;
                    entity.SKU = dto.SKU;
                    entity.BrandId = dto.BrandId;
                    entity.CategoryId = dto.CategoryId;
                    entity.ImageMasterId = dto.ImageMasterId;
                    entity.ModifiedBy = base.GetUser();
                    entity.ModifiedDate = DateTime.Now;
                    _product.Update(entity);
                }
                else
                {
                    Product entity = new Product()
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        SKU = dto.SKU,
                        BrandId = dto.BrandId,
                        CategoryId = dto.CategoryId,
                        ImageMasterId = dto.ImageMasterId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = base.GetUser()
                    };
                    _product.Add(entity);
                }
            }
            catch (Exception e)
            {
                response.Value = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                return response;
            }

            response.Value = HttpStatusCode.OK;
            response.IsSuccess = true;

            return response;
        }

        [HttpGet]
        [Route("List")]
        public async Task<List<ProductDto>> List()
        {
            List<ProductDto> list = new List<ProductDto>();
     
            try
            {
                var allProducts = await _product.GetListAsync(x => x.IsActive);
                foreach (var entity in allProducts)
                {
                    ProductDto model = new ProductDto()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        SKU = entity.SKU,
                        BrandId = entity.BrandId,
                        CategoryId = entity.CategoryId,
                        ImageMasterId = entity.ImageMasterId
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

        [HttpPost]
        [Route("ListByIds")]
        public async Task<List<ProductDto>> List([FromBody] IReadOnlyCollection<int> productIds)
        {
            List<ProductDto> list = new List<ProductDto>();
          
            try
            {
                var allProducts = await _product.GetListAsync(x => x.IsActive && productIds.Contains(x.Id));
                foreach (var entity in allProducts)
                {
                    ProductDto model = new ProductDto()
                    {
                        Id = entity.Id,
                        Name = entity.Name,
                        Description = entity.Description,
                        SKU = entity.SKU,
                        BrandId = entity.BrandId,
                        CategoryId = entity.CategoryId,
                        ImageMasterId = entity.ImageMasterId
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
        
        [HttpGet]
        [Route("GetOne/{id}")]
        [AllowAnonymous]
        public async Task<ProductDto> GetOne(int id)
        {
            var product = await _product.GetTAsync(x => x.IsActive == true && x.Id == id);

            ProductDto model = new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                SKU = product.SKU,
                BrandId = product.BrandId,
                CategoryId = product.CategoryId,
                ImageMasterId = product.ImageMasterId
            };

            return model;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response<HttpStatusCode>> Delete(int id)
        {
            var product = await _product.GetTAsync(x => x.IsActive == true && x.Id == id);
            product.IsActive = false;
            _product.Update(product);

            return new Response<HttpStatusCode>(HttpStatusCode.OK);
        }
    }
}
