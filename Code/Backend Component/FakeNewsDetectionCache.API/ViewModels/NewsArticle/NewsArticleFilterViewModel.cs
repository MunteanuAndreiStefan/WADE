using System;
using FakeNewsDetectionCache.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace FakeNewsDetectionCache.API.ViewModels.NewsArticle
{
    public class NewsArticleFilterViewModel : BaseFilterViewModel<Entities.Models.NewsArticle>
    {
        public string FilterBySource { get; set; }
        public Guid? FilterByUserId { get; set; }

        public List<string> FilterByUrls { get; set; }



        public override System.Linq.Expressions.Expression<Func<Entities.Models.NewsArticle, bool>> GetFilter()
        {
            Expression<Func<Entities.Models.NewsArticle, bool>> filter = x => true;
            Expression<Func<Entities.Models.NewsArticle, bool>> _filterBySource = x => x.Source == FilterBySource;
            Expression<Func<Entities.Models.NewsArticle, bool>> _filterByUserId = x => x.UserId == FilterByUserId;


            Expression<Func<Entities.Models.NewsArticle, bool>> test = x => true && x.UserId == FilterByUserId;


            if (FilterBySource != "" && FilterBySource != null)
                filter = Expression.Lambda<Func<Entities.Models.NewsArticle, bool>>(Expression.AndAlso(filter.Body, _filterBySource.Body), filter.Parameters[0]);

            if (FilterByUserId.HasValue)
                filter = Expression.Lambda<Func<Entities.Models.NewsArticle, bool>>(Expression.AndAlso(filter.Body, _filterByUserId.Body), filter.Parameters[0]);

            if ((FilterByUrls != null) && (FilterByUrls.Count > 0))
            {
                Expression<Func<Entities.Models.NewsArticle, bool>> _filterByUrls = x => FilterByUrls.Contains(x.Source);
                filter = Expression.Lambda<Func<Entities.Models.NewsArticle, bool>>(Expression.AndAlso(filter.Body, _filterByUserId.Body), filter.Parameters[0]);
            }
            //return filter;
            return filter;
        }

        public override async Task<List<Entities.Models.NewsArticle>> ApplyFilter(IQueryable<Entities.Models.NewsArticle> entities)
        {
            var filteredData = entities.Where(x => true);

            if (FilterBySource != "" && FilterBySource != null)
                filteredData = filteredData.Where(x => x.Source == FilterBySource);

            if (FilterByUserId.HasValue)
                filteredData = filteredData.Where(x => x.UserId == FilterByUserId);

            if ((FilterByUrls != null) && (FilterByUrls.Count > 0))
                filteredData = filteredData.Where(x => FilterByUrls.Contains(x.Source));

            return filteredData.ToList();
        }

    }
}
