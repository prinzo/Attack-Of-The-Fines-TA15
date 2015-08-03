using System;
using System.Collections.Generic;
using System.DirectoryServices;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;

namespace FineBot.API.LdapApi
{
    public class LdapApi : ILdapApi
    {
        private readonly IRepository<User, Guid> userRepository;
        private readonly IUserMapper userMapper;
        //TODO: Move to config
        private const string LDAP = "LDAP://192.168.2.3";

        public LdapApi(
            IRepository<User, Guid> userRepository,
            IUserMapper userMapper
            )
        {
            this.userRepository = userRepository;
            this.userMapper = userMapper;
        }

        public List<UserModel> GetAllDomainUsers(string domainName, string password)
        {
            var domainUsers = new List<UserModel>();

            try
            {
                var directoryEntry = new DirectoryEntry(LDAP, domainName, password);
                var directorySearcher = new DirectorySearcher(directoryEntry)
                {
                    Filter = "(&(objectClass=user)(objectCategory=person))"
                };
                directorySearcher.PropertiesToLoad.Add("samaccountname");
                directorySearcher.PropertiesToLoad.Add("mail");
                directorySearcher.PropertiesToLoad.Add("usergroup");
                directorySearcher.PropertiesToLoad.Add("displayname"); 
                var searchResultsCollection = directorySearcher.FindAll();

                for (var counter = 0; counter < searchResultsCollection.Count; counter++)
                {
                    var result = searchResultsCollection[counter];
                    if (!result.Properties.Contains("samaccountname") || !result.Properties.Contains("mail") ||
                        !result.Properties.Contains("displayname")) continue;
                    var userModel = new UserModel
                    {
                        EmailAddress = result.Properties["mail"][0].ToString(),
                        DisplayName = result.Properties["displayname"][0].ToString()
                    };
                    domainUsers.Add(userModel);
                }

            }
            catch (DirectoryServicesCOMException ex)
            {
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return domainUsers;
        }

        public UserModel AuthenticateAgainstDomain(string domainName, string password)
        {
            try
            {
                var directoryEntry = new DirectoryEntry(LDAP, domainName, password);
                var searchAdForUser = new DirectorySearcher(directoryEntry) { Filter = "(&(objectClass=user)(anr=" + domainName + "))" };
                var retrievedUser = searchAdForUser.FindOne();

                var ldapEmailAddress = retrievedUser.Properties["mail"][0].ToString();

                var existingUser = this.userRepository.Find(new UserSpecification().WithEmailAddress(ldapEmailAddress));

                if(existingUser == null)
                {
                    existingUser = new User { EmailAddress = ldapEmailAddress };
                }

                existingUser.DisplayName = String.Format("{0}{1}", retrievedUser.Properties["givenname"][0],
                    retrievedUser.Properties["sn"][0]);

                this.userRepository.Save(existingUser);

                var userModel = this.userMapper.MapToModel(existingUser);

                //    new UserModel()
                //{
                //    Title = retrievedUser.Properties["title"][0].ToString(),
                //    Mobile = retrievedUser.Properties["mobile"][0].ToString(),
                //    EmailAddress = ldapEmailAddress,
                //    Name = retrievedUser.Properties["givenname"][0].ToString(),
                //    Surname = retrievedUser.Properties["sn"][0].ToString()
                //};
               
                return userModel;
            }
            catch (DirectoryServicesCOMException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public UserModel MapSlackModelToLdapModel(UserModel ldapModel, UserModel slackModel)
        {
            //@Kurt, can you suggest a better way?
            ldapModel.SlackId = slackModel.SlackId;
            ldapModel.Id = slackModel.Id;
            ldapModel.DisplayName = slackModel.DisplayName;
            ldapModel.AwardedFineCount = slackModel.AwardedFineCount;
            ldapModel.PendingFineCount = slackModel.PendingFineCount;
            ldapModel.Fines = slackModel.Fines;
            ldapModel.Image = slackModel.Image;
            return ldapModel;
        }
    }
}
