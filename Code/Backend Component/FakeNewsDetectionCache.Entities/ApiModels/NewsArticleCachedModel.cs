using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Entities
{
    public class NewsArticleCachedModel
    {
        public string Source { get; set; }

        public int CredibilityScore { get; set; }

        public int PositiveVoteCount { get; set; }

        public int NegativeVoteCount { get; set; }
    }
}
