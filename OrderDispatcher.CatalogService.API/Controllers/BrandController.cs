using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderDispatcher.CatalogService.API.Base;
using OrderDispatcher.CatalogService.API.Models;
using OrderDispatcher.CatalogService.Core.Entities;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Dal.Concrete.EntityFramework;
using OrderDispatcher.CatalogService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.API.Controllers
{
    [Route("api/catalog/brand")]
    [Produces("application/json")]
    public class BrandController : APIControllerBase
    {
        private readonly IBrand _brand;

        public BrandController(IBrand brand)
        {
            _brand = brand;
        }

        [HttpPost]
        [Route("Save")]
        public Response<HttpStatusCode> Save([FromBody] BrandDto dto)
        {
            Response<HttpStatusCode> response = new Response<HttpStatusCode>();
            try
            {
                if (dto.Id != 0)
                {
                    Brand entity = _brand.GetT(x => x.Id == dto.Id);
                    entity.Name = dto.Name;
                    entity.Description = dto.Description;
                    entity.ModifiedBy = base.GetUser();
                    entity.ModifiedDate = DateTime.Now;
                    _brand.Update(entity);
                }
                else
                {
                    Brand entity = new Brand()
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = base.GetUser()
                    };
                    _brand.Add(entity);
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
        public async Task<List<BrandDto>> List()
        {
            List<BrandDto> list = new List<BrandDto>();

            try
            {
                var allCategories = await _brand.GetListAsync(x => x.IsActive);
                foreach (var entity in allCategories)
                {
                    BrandDto model = new BrandDto()
                    {
                        Name = entity.Name,
                        Id = entity.Id,
                        Description = entity.Description
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
        public async Task<BrandDto> GetOne(int id)
        {
            var brand = await _brand.GetTAsync(x => x.IsActive == true && x.Id == id);

            BrandDto model = new BrandDto()
            {
                Name = brand.Name,
                Description = brand.Description,
                Id = brand.Id,
            };

            return model;
        }


        [HttpPost]
        [Route("Delete")]
        public async Task<Response<HttpStatusCode>> Delete(int id)
        {
            var Brand = await _brand.GetTAsync(x => x.IsActive == true && x.Id == id);
            Brand.IsActive = false;
            _brand.Update(Brand);

            return new Response<HttpStatusCode>(HttpStatusCode.OK);
        }
    }
}