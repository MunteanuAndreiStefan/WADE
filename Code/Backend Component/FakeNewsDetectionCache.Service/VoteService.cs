using FakeNewsDetectionCache.Entities;
using FakeNewsDetectionCache.Entities.Models;
using FakeNewsDetectionCache.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Service
{
    public class VoteService : CrudService<Vote, Repository>, IVoteService
    {
        private IApplicationUserService applicationUserService;

        private INewsArticleService newsArticleService;

        public VoteService(IApplicationUserService ApplicationUserService,
                        INewsArticleService NewsArticleService,
                        Repository dbContext) : base(dbContext)
        {
            applicationUserService = ApplicationUserService;
            newsArticleService = NewsArticleService;
        }

        public async Task<Guid> AddOrUpdate(VoteViewModel model)
        {
            var userId=(await applicationUserService.GetAsQueriable()).Where(x => x.UserId == model.ApplicationUserId).Select(x=>x.Id).FirstOrDefault();
            if (userId == Guid.Empty)
                userId = await applicationUserService.Add(new ApplicationUser
                {
                    UserId = model.ApplicationUserId,
                    Username = model.ApplicationUserId,
                    CredibilityScore = 50
                });

            var newsArtileId = (await newsArticleService.GetAsQueriable()).Where(n => n.Source == model.NewsArticleUrl).Select(q=>q.Id).FirstOrDefault();
            if (newsArtileId == Guid.Empty)
                throw new Exception("Invalid Article Url");

            var vote=(await GetAsQueriable()).Where(x => x.ApplicationUserId==userId).FirstOrDefault();
            if (vote == null)
               return await Add(new Vote { ApplicationUserId = userId, NewsArticleId = newsArtileId, IsTrue = model.IsTrue });
            else
            {
                vote.IsTrue = model.IsTrue;
                return await Update(vote);
            }
        }
    }
}
