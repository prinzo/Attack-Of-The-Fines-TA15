﻿using System.Collections.Generic;
using FineBot.API.UsersApi;

namespace FineBot.API.LdapApi
{
    public interface ILdapApi
    {
        List<UserModel> GetAllDomainUsers(string domainName, string password);
        UserModel AuthenticateAgainstDomain(string domainName, string password);
        UserModel MapSlackModelToLdapModel(UserModel ldapModel, UserModel slackModel);
    }
}
