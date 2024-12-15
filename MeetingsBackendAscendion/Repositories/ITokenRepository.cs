using Microsoft.AspNetCore.Identity;

namespace MeetingsBackendAscendion.Repositories;

    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user);
    }
