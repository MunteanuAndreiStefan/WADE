using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeNewsDetectionCache.API.ViewModels;
using FakeNewsDetectionCache.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FakeNewsDetectionCache.API.ViewModels.NewsArticle;
using Microsoft.AspNetCore.Authorization;
using FakeNewsDetectionCache.Authentication;
using FakeNewsDetectionCache.Authentication.Authorization;

namespace FakeNewsDetectionCache.API.Controllers
{
    //[Log]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsArticlesController : ControllerBase
    {

        protected INewsArticleService NewsArticleService;

        public NewsArticlesController(INewsArticleService newsArticleService)
        {
            NewsArticleService = newsArticleService;
        }


        [HttpGet]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> Get()
        {
            var items = await NewsArticleService.GetAll();
            if (items.Count == 0)
                Response.StatusCode = StatusCodes.Status404NotFound;

            return new JsonResult(items);
        }

        [HttpGet("{url}")]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> Get([FromQuery]string url)
        {
            var item = await NewsArticleService.GetArticleByUrl(url);
            

            return new JsonResult(item);
        }


        [HttpPost("{model}")]
        [Authorize(Policy = Policies.OnlyExtension)]
        public async Task<JsonResult> GetFiltered(NewsArticleFilterViewModel model)
        {
            var items = await model.ApplyFilter(await NewsArticleService.GetAsQueriable());
            if (items.Count == 0)
                Response.StatusCode = StatusCodes.Status404NotFound;
            return new JsonResult(items);
        }

        [HttpPost]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Post(NewsArticleViewModel model)
        {
            await NewsArticleService.Add(model.ToEntity());
        }

        [HttpPut]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Put(NewsArticleViewModel model)
        {
            try
            {
                await NewsArticleService.Update(model.ToEntity());
            }
            catch {
                Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        [HttpDelete("{Id}")]
        [Authorize(Policy = Policies.OnlyDevelopers)]
        public async Task Delete(Guid Id)
        {
            var entityToDelete = (await NewsArticleService.GetByFilter(x => x.Id == Id)).FirstOrDefault();

            if (entityToDelete != null)
                await NewsArticleService.Delete(entityToDelete);
            else
                Response.StatusCode = StatusCodes.Status404NotFound;
        }

    }
}

