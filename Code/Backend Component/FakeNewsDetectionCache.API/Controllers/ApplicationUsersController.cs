using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeNewsDetectionCache.API.ViewModels.ApplicationUser;
using FakeNewsDetectionCache.Authentication.Authorization;
using FakeNewsDetectionCache.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeNewsDetectionCache.API.Controllers
{
    //[Log]
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        protected IApplicationUserService ApplicationUserService;

        public ApplicationUsersController(IApplicationUserService applicationUserService)
        {
            ApplicationUserService = applicationUserService;
        }

        [HttpGet]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> Get()
        {
            var items = await ApplicationUserService.GetAll();
            if (items.Count == 0)
                Response.StatusCode = StatusCodes.Status404NotFound;
            return new JsonResult(items);
        }

        [HttpGet("Id")]
        public async Task<JsonResult> GetById(Guid Id)
        {
            var item = (await ApplicationUserService.GetAsQueriable()).Where(q=>q.Id==Id).FirstOrDefault();
            if (item==null)
                Response.StatusCode = StatusCodes.Status404NotFound;
            return new JsonResult(item);
        }

        [HttpPost("{model}")]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> GetFiltered(ApplicationUserFilterViewModel model)
        {
            var items = await model.ApplyFilter(await ApplicationUserService.GetAsQueriable());
            if (items.Count == 0)
                Response.StatusCode = StatusCodes.Status404NotFound;
            return new JsonResult(items);
        }

        [HttpPost]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Post(ApplicationUserViewModel model)
        {
            await ApplicationUserService.Add(model.ToEntity());
        }

        [HttpPut]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Put(ApplicationUserViewModel model)
        {
            try
            {
                await ApplicationUserService.Update(model.ToEntity());
            }
            catch { Response.StatusCode = StatusCodes.Status400BadRequest; }
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Delete(Guid Id)
        {
            var entityToDelete = (await ApplicationUserService.GetByFilter(x => x.Id == Id)).FirstOrDefault();

            if (entityToDelete != null)
                await ApplicationUserService.Delete(entityToDelete);
            else
                Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}