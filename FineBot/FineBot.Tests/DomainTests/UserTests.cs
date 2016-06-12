using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Entities;
using FineBot.Enums;
using NUnit.Framework;
using SharpTestsEx;

namespace FineBot.Tests.DomainTests
{
    [TestFixture]
    public class UserTests
    {
        [Test]
        public void GivenANewUser_IssueFine_WithoutSeconder_IssuesANewPendingFine()
        {
            // Arrange:
            User user = new User();
            var issuerId = new Guid();

            // Act:
            user.IssueFine(issuerId, "for testing", PlatformType.Slack);

            // Assert:
            user.Fines.Should().Not.Be.Empty();
            user.Fines.First().Pending.Should().Be.True();
            user.Fines.First().IssuerId.Should().Be.EqualTo(issuerId);
        }

        [Test]
        public void GivenANewUser_IssueFine_WithSeconder_IssuesANewFineWhichIsNotPending()
        {
            // Arrange:
            User user = new User();
            var issuerId = new Guid();
            var seconderId = new Guid();

            // Act:
            user.IssueFine(issuerId, "for testing", PlatformType.Slack, seconderId);

            // Assert:
            user.Fines.Should().Not.Be.Empty();
            user.Fines.First().Pending.Should().Be.False();
            user.Fines.First().IssuerId.Should().Be.EqualTo(issuerId);
            user.Fines.First().SeconderId.Should().Be.EqualTo(seconderId);
        }

        [Test]
        public void GivenAUserWithFines_GetFineById_ReturnsCorrectFine()
        {
            // Arrange:
            var fineId = new Guid();
            User user = new User()
                        {
                            Fines = new List<Fine>
                                    {
                                        new Fine(){Id = fineId},
                                        new Fine()
                                    }
                        };

            // Act:
            var fine = user.GetFineById(fineId);

            // Assert:
            fine.Should().Not.Be.Null();
            fine.Id.Should().Be.EqualTo(fineId);
        }

        [Test]
        public void GivenAUserWithFines_GetOldestPendingFine_ReturnsCorrectFine()
        {
            // Arrange:
            var oldestFine = new Fine{Id = new Guid(), AwardedDate = DateTime.Now};

            User user = new User()
            {
                Fines = new List<Fine>
                                    {
                                        oldestFine,
                                        new Fine{AwardedDate = DateTime.Now.AddHours(1)},
                                        new Fine{SeconderId = new Guid()}
                                    }
            };

            // Act:
            var fine = user.GetOldestPendingFine();

            // Assert:
            fine.Should().Not.Be.Null();
            fine.Should().Be.EqualTo(oldestFine);
        }

        [Test]
        public void GivenAUserWithFines_GetNewestPendingFine_ReturnsCorrectFine()
        {
            // Arrange:
            var oldestFine = new Fine { Id = new Guid(), AwardedDate = DateTime.Now };
            var newestFine = new Fine{AwardedDate = DateTime.Now.AddHours(1)};

            User user = new User()
            {
                Fines = new List<Fine>
                                    {
                                        oldestFine,
                                        newestFine,
                                        new Fine{SeconderId = new Guid()}
                                    }
            };

            // Act:
            var fine = user.GetNewestPendingFine();

            // Assert:
            fine.Should().Not.Be.Null();
            fine.Should().Be.EqualTo(newestFine);
        }
    }
}