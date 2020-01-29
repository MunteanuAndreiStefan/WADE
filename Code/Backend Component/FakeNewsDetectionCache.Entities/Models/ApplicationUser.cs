using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;

namespace FakeNewsDetectionCache.Entities.Models
{
    public partial class ApplicationUser:Entity
    {
        public ApplicationUser()
        {
            Votes = new HashSet<Vote>();
        }

        //public int Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public int? CredibilityScore { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
