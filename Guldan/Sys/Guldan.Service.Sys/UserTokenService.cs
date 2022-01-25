using System;
using System.Security.Claims;
using Guldan.Data;
using Guldan.IService.Sys;

namespace Guldan.Service.Sys
{
    public class UserTokenService : IUserTokenService
    {
        public string Create(Claim[] claims)
        { 
            throw new NotImplementedException();
        }

        public Claim[] Decode(string jwtToken)
        {
            throw new NotImplementedException();
        }
    }
}
