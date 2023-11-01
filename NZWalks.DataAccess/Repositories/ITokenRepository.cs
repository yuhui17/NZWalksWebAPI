
using Microsoft.AspNetCore.Identity;
using NZWalks.Model.Models.Domain;

namespace NZWalks.NZWalksDataAccess.Repositories
{

    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
