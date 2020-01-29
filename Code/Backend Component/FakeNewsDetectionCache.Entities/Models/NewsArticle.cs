using System;
using System.Collections.Generic;

namespace FakeNewsDetectionCache.Entities.Models
{
    public partial class NewsArticle:Entity
    {
        public NewsArticle()
        {
            Votes = new HashSet<Vote>();
        }

        //public int Id { get; set; }
        public string Source { get; set; }
        public int? CredibilityScore { get; set; }
        public Guid UserId { get; set; }

        public virtual TwitterUser User { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
