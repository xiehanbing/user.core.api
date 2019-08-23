using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace User.Core.Identity.Authentication
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(o => o.Type == "sub")?.Value??"";
            if(!int.TryParse(subjectId,out int intUserId))
                throw new ArgumentException("invalid subject identifier");
            context.IssuedClaims = context.Subject.Claims.ToList();
            return Task.CompletedTask;
            
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subject= context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subjectId = subject.Claims.FirstOrDefault(o => o.Type == "sub")?.Value ?? "";
            if (!int.TryParse(subjectId, out int intUserId))
                throw new ArgumentException("invalid subject identifier");
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}