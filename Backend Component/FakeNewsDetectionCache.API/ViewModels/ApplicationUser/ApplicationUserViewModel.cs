using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.API.ViewModels.ApplicationUser
{
    public class ApplicationUserViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string Username { get; set; }
        public int? CredibilityScore { get; set; }

        public ApplicationUserViewModel()
        {

        }

        public ApplicationUserViewModel(Entities.Models.ApplicationUser applicationUser)
        {
            Username = applicationUser.Username;
            CredibilityScore = applicationUser.CredibilityScore;
        }

        public Entities.Models.ApplicationUser ToEntity()
        {
            return new Entities.Models.ApplicationUser
            {
                Id = Id??Guid.Empty,
                Username = Username,
                UserId=Username,
                CredibilityScore = CredibilityScore
            };
        }
    }
}
