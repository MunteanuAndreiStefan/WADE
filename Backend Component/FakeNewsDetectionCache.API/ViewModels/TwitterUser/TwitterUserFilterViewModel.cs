using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FakeNewsDetectionCache.Entities.Models;

namespace FakeNewsDetectionCache.API.ViewModels.TwitterUser
{
    public class TwitterUserFilterViewModel : BaseFilterViewModel<Entities.Models.TwitterUser>
    {
        public string FilterByUsername { get; set; }



        public override async Task<List<Entities.Models.TwitterUser>> ApplyFilter(IQueryable<Entities.Models.TwitterUser> entities)
        {
            var filteredData = entities.Where(x => true);

            if (FilterByUsername != "" && FilterByUsername != null)
                filteredData = filteredData.Where(x => x.Username == FilterByUsername);

            return filteredData.ToList();
        }

        public override Expression<Func<Entities.Models.TwitterUser, bool>> GetFilter()
        {
            throw new NotImplementedException();
        }
    }
}
