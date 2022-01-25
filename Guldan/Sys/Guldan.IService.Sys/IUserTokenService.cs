using System;
using System.Security.Claims;

namespace Guldan.IService.Sys
{
    public interface IUserTokenService
    {
        string Create(Claim[] claims);

        Claim[] Decode(string jwtToken);

    }
}
