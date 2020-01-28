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
        public VoteService(IApplicationUserService ApplicationUserService, Repository dbContext) : base(dbContext)
        {
            applicationUserService = ApplicationUserService;
        }

        public async Task<Guid> AddOrUpdate(VoteViewModel model)
        {
            var userId=(await applicationUserService.GetAsQueriable()).Where(x => x.UserId == model.ApplicationUserId).Select(x=>x.Id).FirstOrDefault();
            if (userId == Guid.Empty)
                throw new Exception("Invalid User Id");

            var vote=(await GetAsQueriable()).Where(x => x.ApplicationUserId==userId).FirstOrDefault();
            if (vote == null)
               return await Add(new Vote { ApplicationUserId = userId, NewsArticleId = model.NewsArticleId, IsTrue = model.IsTrue });
            else
            {
                vote.IsTrue = model.IsTrue;
                return await Update(vote);
            }
        }
    }
}
