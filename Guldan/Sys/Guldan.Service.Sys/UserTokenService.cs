using System;
using System.Security.Claims;
using Guldan.Data;
using Guldan.IService.Sys;
using Guldan.Service.Factory;

namespace Guldan.Service.Sys
{
    public class UserTokenService : IUserTokenService
    {
        public string Create(Claim[] claims)
        {
            using (var context = BizContextFactory.GetBizContext())
            {
                 




            }

            return "";
        }

        public Claim[] Decode(string jwtToken)
        {
            throw new NotImplementedException();
        }
    }
}
