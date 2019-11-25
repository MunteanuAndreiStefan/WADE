using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.API.ViewModels.ApplicationUser
{
    public class ApplicationUserFilterViewModel: BaseFilterViewModel<Entities.Models.ApplicationUser>
    {

        public string FilterByUsername { get; set; }
        public override async Task<List<Entities.Models.ApplicationUser>> ApplyFilter(IQueryable<Entities.Models.ApplicationUser> entities)
        {
            var filteredData = entities.Where(x => true);

            if (FilterByUsername != "" && FilterByUsername != null)
                filteredData = filteredData.Where(x => x.Username == FilterByUsername);

            return filteredData.ToList();
        }

        public override Expression<Func<Entities.Models.ApplicationUser, bool>> GetFilter()
        {
            throw new NotImplementedException();
        }
    }
}
