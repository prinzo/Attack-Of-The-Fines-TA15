using System;
using System.Collections.Generic;

namespace FineBot.API.UsersApi
{
    public interface IUserApi
    {
        UserModel GetUserById(Guid id);

        UserModel GetUserBySlackId(string slackId);

        UserModel GetUserByEmail(string email);

        IList<UserModel> GetValidFineIssuers(IList<UserModel> users);

        void UpdateUser(UserModel userModel);

        void UpdateUserImage(Guid userId, string image);

        string[] GetUserNameAndSurnameFromEmail(string email);

        int GetNumberOfFinesForUserById(Guid id);

        int GetOutstandingFineCountForUser(Guid userId);

        int GetSuccessfullyIssuedFineCountForUser(Guid userId);

        int GetFinesSecondedByUserCount(Guid userId);

        UserModel RegisterUserBySlackId(string slackId);

        List<UserModel> GetLeaderboard(int number);

        List<UserModel> GetLeaderboardToday(int number);

        List<UserModel> GetLeaderboardForThisWeek(int number);

        List<UserModel> GetLeaderboardForThisMonth(int number);

        List<UserModel> GetLeaderboardForThisYear(int number);

        List<UserModel> GetUsersWithPendingFines();

        List<UserModel> GetAllUsers();

        UserModel RegisterUserByEmail(string email);

        UserStatisticModel GetStatisticsForUser(Guid id);

        bool IsValidFineIssuer(Guid issuerId);
    }
}