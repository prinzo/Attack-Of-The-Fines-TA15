using System;
using System.Collections.Generic;
using FineBot.API.FinesApi;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.Entities;
using FineBot.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;
using SharpTestsEx;

namespace FineBot.Tests.API
{
    [TestFixture]
    public class FinesApiTests
    {
        [Test]
        public void SecondOldestPendingFine_SecondsCorrectFine()
        {
            // Arrange:
            IRepository<User, Guid> userRepository = MockRepository.GenerateMock<IRepository<User, Guid>>();
            IFineMapper fineMapper = MockRepository.GenerateMock<IFineMapper>();
            IUserMapper userMapper = MockRepository.GenerateMock<IUserMapper>();

            var fine = new Fine{AwardedDate = DateTime.Now};
            var user = new User{Fines = new List<Fine>{fine, new Fine{AwardedDate = DateTime.Now.AddMinutes(1)}}};

            userRepository.Stub(x => x.FindAll(null)).IgnoreArguments().Return(new List<User> { user });

            var userModel = new UserModel();
            userMapper.Stub(x => x.MapToModelShallow(user)).Return(userModel);

            FineApi fineApi = new FineApi(userRepository, fineMapper, userMapper);

            // Pre-Assert:
            fine.Pending.Should().Be.True();

            // Act:
            fineApi.SecondOldestPendingFine(new Guid());

            // Assert:
            fine.Pending.Should().Be.False();

            userRepository.AssertWasCalled(x => x.Save(user));
            fineMapper.AssertWasCalled(x => x.MapToModelWithUser(fine, userModel));
        }
    }
}