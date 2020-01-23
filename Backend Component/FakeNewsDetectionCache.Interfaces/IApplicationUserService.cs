using FakeNewsDetectionCache.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Interfaces
{
    public interface IApplicationUserService: ICrudService<ApplicationUser>
    {
        public Task<bool> UserExists(string userId);
    }
}
