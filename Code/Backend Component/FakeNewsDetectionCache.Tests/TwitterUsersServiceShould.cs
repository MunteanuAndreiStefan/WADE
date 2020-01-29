using FakeNewsDetectionCache.Entities.Models;
using FakeNewsDetectionCache.Interfaces;
using FakeNewsDetectionCache.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FakeNewsDetectionCache.Tests
{
    public class TwitterUsersServiceShould
    {
        private ITwitterUserService twitterUserService;
        private Guid userId;
        public TwitterUsersServiceShould()
        {
            var options = new DbContextOptionsBuilder<Repository>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database" + this.GetHashCode())
                .Options;
            var ctx = new Repository(options);
            twitterUserService = new TwitterUserService(ctx);


        }

        [Fact]
        public async Task ShouldAddTwitterUser()
        {
            userId = await twitterUserService.Add(new TwitterUser { Username = "TestUser", CredibilityScore = 69, });
            var result = (await twitterUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault() ;

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldReadTwitterUser()
        {
            userId = await twitterUserService.Add(new TwitterUser { Username = "TestUser", CredibilityScore = 69, });
            var result = (await twitterUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldUpdateTwitterUser()
        {
            userId = await twitterUserService.Add(new TwitterUser { Username = "TestUser", CredibilityScore = 69, });
            var entityToUpdate = (await twitterUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();
            entityToUpdate.CredibilityScore = 10;
            await twitterUserService.Update(entityToUpdate);
            var result = (await twitterUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();


            Assert.Equal(10, result.CredibilityScore ?? 0);
        }

        [Fact]
        public async Task ShoulDeleteTwitterUser()
        {
            userId = await twitterUserService.Add(new TwitterUser { Username = "TestUser", CredibilityScore = 69, });
            var entityToDelete = (await twitterUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();

            await twitterUserService.Delete(entityToDelete);
            var result = (await twitterUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();


            Assert.Null(result);
        }
    }
}
