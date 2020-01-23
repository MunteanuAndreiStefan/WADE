using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Entities
{
    public class VoteViewModel
    {

        //public Guid? Id { get; set; }
        [Required]
        public string ApplicationUserId { get; set; }
        [Required]
        public Guid NewsArticleId { get; set; }

        public bool IsTrue { get; set; }

        public VoteViewModel()
        {

        }


    }
}
