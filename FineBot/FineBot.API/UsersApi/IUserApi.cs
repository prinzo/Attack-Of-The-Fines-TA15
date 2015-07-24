using System;
using System.Collections.Generic;

namespace FineBot.API.UsersApi
{
    public interface IUserApi
    {
        UserModel GetUserById(Guid id);

        UserModel GetUserBySlackId(string slackId);

        UserModel CreateUserFromSlackId(string slackUserId);

        int GetNumberOfFinesForUserById(Guid id);

        UserModel RegisterUser(string slackId);

        List<UserModel> GetLeaderboard(int number);
    }
}