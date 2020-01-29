using FakeNewsDetectionCache.Entities.Models;
using FakeNewsDetectionCache.Interfaces;
using FakeNewsDetectionCache.Service;
using FakeNewsDetectionCache.Service.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FakeNewsDetectionCache.Tests
{
    public class NewsArticlesServiceShould
    {
        private ITwitterUserService twitterUserService;
        private INewsArticleService newsArticleService;
        private Guid userId;
        public NewsArticlesServiceShould()
        {
            var options = new DbContextOptionsBuilder<Repository>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database"+this.GetHashCode())
                .Options;
            var ctx = new Repository(options);
            twitterUserService = new TwitterUserService(ctx);

            IOptions<ProcessingUnitConfig> processingUnitConfig = Options.Create<ProcessingUnitConfig>(new ProcessingUnitConfig());

            newsArticleService = new NewsArticleService(twitterUserService, processingUnitConfig, ctx);


        }


        private async Task addTwitterUser()
        {
            userId = await twitterUserService.Add(new TwitterUser { Username = "TestUser", CredibilityScore = 69, });
        }

        private async Task addNewsArticle()
        {
            await newsArticleService.Add(new NewsArticle { Source = "articleFromMemory", CredibilityScore = 69, UserId = userId });
        }

        [Fact]
        public async Task ShouldAddNewsArticle()
        {
            await addTwitterUser();

            var TestObject = new NewsArticle { Source = "testUrl", CredibilityScore = 69, UserId = userId };

            await newsArticleService.Add(TestObject);

            var result = await newsArticleService.GetByFilter(x => x.Source == TestObject.Source);

            Assert.NotNull(result);

        }

        [Fact]
        public async Task ShouldReadNewsArticle()
        {
            await addTwitterUser();
            await addNewsArticle();
            var result = await newsArticleService.GetByFilter(x => x.Source == "articleFromMemory");

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldUpdateNewsArticle()
        {
            await addTwitterUser();
            await addNewsArticle();
            var entityToUpdate= (await newsArticleService.GetByFilter(x => x.Source == "articleFromMemory")).FirstOrDefault();
            entityToUpdate.CredibilityScore = 10;
            await newsArticleService.Update(entityToUpdate);
            var result= (await newsArticleService.GetByFilter(x => x.Source == "articleFromMemory")).FirstOrDefault();


            Assert.Equal(10, result.CredibilityScore??0);
        }

        [Fact]
        public async Task ShoulDeleteNewsArticle()
        {
            await addTwitterUser();
            await addNewsArticle();
            var entityToDelete = (await newsArticleService.GetByFilter(x => x.Source == "articleFromMemory")).FirstOrDefault();
            
            await newsArticleService.Delete(entityToDelete);
            var result = (await newsArticleService.GetByFilter(x => x.Source == "articleFromMemory")).FirstOrDefault();


            Assert.Null(result);
        }
    }
}
