using System;

namespace FineBot.API.UsersApi
{
    public interface IUserApi
    {
        UserModel GetUserById(Guid id);

        UserModel GetUserBySlackId(string slackId);

        UserModel CreateUserFromSlackId(string slackUserId);

        int GetNumberOfFinesForUserById(Guid id);

    }
}