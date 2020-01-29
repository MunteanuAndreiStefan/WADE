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
    public class ApplicationUserServiceShould
    {
        private IApplicationUserService applicationUserService;
        private Guid userId;
        public ApplicationUserServiceShould()
        {
            var options = new DbContextOptionsBuilder<Repository>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database" + this.GetHashCode())
                .Options;
            var ctx = new Repository(options);
            applicationUserService = new ApplicationUserService(ctx);


        }

        [Fact]
        public async Task ShouldAddApplicationUser()
        {
            userId = await applicationUserService.Add(new ApplicationUser { Username = "TestUser", CredibilityScore = 69, });
            var result = (await applicationUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldReadApplicationUser()
        {
            userId = await applicationUserService.Add(new ApplicationUser { Username = "TestUser", CredibilityScore = 69, });
            var result = (await applicationUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task ShouldUpdateApplicationUser()
        {
            userId = await applicationUserService.Add(new ApplicationUser { Username = "TestUser", CredibilityScore = 69, });
            var entityToUpdate = (await applicationUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();
            entityToUpdate.CredibilityScore = 10;
            await applicationUserService.Update(entityToUpdate);
            var result = (await applicationUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();


            Assert.Equal(10, result.CredibilityScore ?? 0);
        }

        [Fact]
        public async Task ShoulDeleteApplicationUser()
        {
            userId = await applicationUserService.Add(new ApplicationUser { Username = "TestUser", CredibilityScore = 69, });
            var entityToDelete = (await applicationUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();

            await applicationUserService.Delete(entityToDelete);
            var result = (await applicationUserService.GetByFilter(x => x.Username == "TestUser")).FirstOrDefault();


            Assert.Null(result);
        }
    }

}
