using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderDispatcher.CatalogService.API.Base;
using OrderDispatcher.CatalogService.API.Models;
using OrderDispatcher.CatalogService.Core.Entities;
using OrderDispatcher.CatalogService.Dal.Abstract;
using OrderDispatcher.CatalogService.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OrderDispatcher.CatalogService.API.Controllers
{
    [Route("api/catalog/category")]
    [Produces("application/json")]
    public class CategoryController : APIControllerBase
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }

        [HttpPost]
        [Route("Save")]
        public Response<HttpStatusCode> Save([FromBody] CategoryDto dto)
        {
            Response<HttpStatusCode> response = new Response<HttpStatusCode>();
            try
            {
                if (dto.Id != 0)
                {
                    Category entity = _category.GetT(x => x.Id == dto.Id);
                    entity.Name = dto.Name;
                    entity.Description = dto.Description;
                    entity.ParentId = dto.ParentId;
                    entity.ModifiedBy = base.GetUser();
                    entity.ModifiedDate = DateTime.Now;
                    _category.Update(entity);
                }
                else
                {
                    Category entity = new Category()
                    {
                        Name = dto.Name,
                        Description = dto.Description,
                        ParentId = dto.ParentId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = base.GetUser()
                    };
                    _category.Add(entity);
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
        public async Task<List<CategoryDto>> List()
        {
            List<CategoryDto> list = new List<CategoryDto>();

            try
            {
                var allCategories = await _category.GetListAsync(x => x.IsActive);
                foreach (var entity in allCategories)
                {
                    CategoryDto model = new CategoryDto()
                    {
                        Name = entity.Name,
                        Id = entity.Id,
                        Description = entity.Description,
                        ParentId = entity.ParentId
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
        public async Task<CategoryDto> GetOne(int id)
        {
            var category = await _category.GetTAsync(x => x.IsActive == true && x.Id == id);

            CategoryDto model = new CategoryDto()
            {
                ParentId = category.ParentId,
                Name = category.Name,
                Description = category.Description,
                Id = category.Id,
            };

            model.ParentCategories = await ParentCategories();

            return model;
        }

        [HttpGet]
        [Route("ParentCategories")]
        [AllowAnonymous]
        public async Task<List<CategoryDto>> ParentCategories()
        {
            List<CategoryDto> list = new List<CategoryDto>();
            var entities = await _category.GetListAsync(x => x.ParentId == 0);
            foreach (var entity in entities)
            {
                CategoryDto model = new CategoryDto()
                {
                    Name = entity.Name,
                    Id = entity.Id,
                    ParentId = entity.ParentId,
                };
                list.Add(model);
            }

            return list;
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<Response<HttpStatusCode>> Delete(int id)
        {
            var category = await _category.GetTAsync(x => x.IsActive == true && x.Id == id);
            category.IsActive = false;
            _category.Update(category);

            return new Response<HttpStatusCode>(HttpStatusCode.OK);
        }
    }
}