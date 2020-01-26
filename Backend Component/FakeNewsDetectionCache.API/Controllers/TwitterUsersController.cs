using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeNewsDetectionCache.API.ViewModels;
using FakeNewsDetectionCache.API.ViewModels.TwitterUser;
using FakeNewsDetectionCache.Authentication;
using FakeNewsDetectionCache.Authentication.Authorization;
using FakeNewsDetectionCache.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FakeNewsDetectionCache.API.Controllers
{
    //[Log]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterUsersController : ControllerBase
    {

        protected ITwitterUserService TwitterUserService;

        public TwitterUsersController(ITwitterUserService tweeterUserService)
        {
            TwitterUserService = tweeterUserService;
        }

        [HttpGet]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> Get()
        {
            var items = await TwitterUserService.GetAll();
            if (items.Count == 0)
                Response.StatusCode = StatusCodes.Status404NotFound;
            return new JsonResult(items);
        }

        [HttpPost("{model}")]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> GetFiltered(TwitterUserFilterViewModel model)
        {
            var items = await model.ApplyFilter(await TwitterUserService.GetAsQueriable());
            return new JsonResult(items);
        }

        [HttpPost]
        [Authorize(Policy = Policies.OnlyProcessingService)]
        public async Task Post(TwitterUserViewModel model)
        {
            if(ModelState.IsValid)
            await TwitterUserService.Add(model.ToEntity());
        }

        [HttpPut]
        [Authorize(Policy = Policies.OnlyProcessingService)]
        public async Task Put(TwitterUserViewModel model)
        {
            try
            {
                await TwitterUserService.Update(model.ToEntity());
            }
            catch { Response.StatusCode = StatusCodes.Status400BadRequest; }
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Delete(Guid Id)
        {
            var entityToDelete = (await TwitterUserService.GetByFilter(x => x.Id == Id)).FirstOrDefault();

            if (entityToDelete != null)
                await TwitterUserService.Delete(entityToDelete);
            else
                Response.StatusCode = StatusCodes.Status404NotFound;
        }

    }
}