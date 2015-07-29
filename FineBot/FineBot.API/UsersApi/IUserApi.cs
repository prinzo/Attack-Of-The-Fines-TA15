using System;
using System.Collections.Generic;

namespace FineBot.API.UsersApi
{
    public interface IUserApi
    {
        UserModel GetUserById(Guid id);

        UserModel GetUserBySlackId(string slackId);

        UserModel GetUserByEmail(string email);

        UserModel UpdateUser(UserModel userModel);

        int GetNumberOfFinesForUserById(Guid id);

        UserModel RegisterUserBySlackId(string slackId);

        List<UserModel> GetLeaderboard(int number);

        List<UserModel> GetLeaderboardToday(int number);

        List<UserModel> GetUsersWithPendingFines();
    }
}