using System;
using System.Collections.Generic;
using System.Linq;
using FineBot.Abstracts;
using FineBot.API.FinesApi;
using FineBot.API.InfrastructureServices;
using FineBot.API.Mappers;
using FineBot.API.Mappers.Interfaces;
using FineBot.API.UsersApi;
using FineBot.BotRunner.Responders;
using FineBot.DataAccess.DataModels;
using FineBot.Entities;
using FineBot.Interfaces;
using FineBot.Specifications;
using MongoDB.Driver;
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
            IRepository<User, UserDataModel, Guid> userRepository = MockRepository.GenerateMock<IRepository<User, UserDataModel, Guid>>();
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository = MockRepository.GenerateMock<IRepository<Payment, PaymentDataModel, Guid>>();
            IFineMapper fineMapper = MockRepository.GenerateMock<IFineMapper>();
            IUserMapper userMapper = MockRepository.GenerateMock<IUserMapper>();
            IPaymentMapper paymentMapper = MockRepository.GenerateMock<IPaymentMapper>();
            IExcelExportService<FineExportModel> excelExportService =
                MockRepository.GenerateMock<IExcelExportService<FineExportModel>>();

            var fine = new Fine{AwardedDate = DateTime.Now};
            var user = new User{Fines = new List<Fine>{fine, new Fine{AwardedDate = DateTime.Now.AddMinutes(1)}}};

            userRepository.Stub(x => x.FindAll(null)).IgnoreArguments().Return(new List<User> { user });

            var userModel = new UserModel();
            userMapper.Stub(x => x.MapToModelShallow(user)).Return(userModel);

            FineApi fineApi = new FineApi(userRepository, paymentRepository, fineMapper, userMapper, paymentMapper, excelExportService);

            // Pre-Assert:
            fine.Pending.Should().Be.True();

            // Act:
            fineApi.SecondOldestPendingFine(Guid.NewGuid());

            // Assert:
            fine.Pending.Should().Be.False();

            userRepository.AssertWasCalled(x => x.Save(user));
            fineMapper.AssertWasCalled(x => x.MapToModelWithUser(fine, userModel));
        }

        [Test]
        public void GivenAListOfNewFines_When_RetrievingLatestFinesForFeed_Then_TheFinesShouldBeRetrieved() {
            IRepository<User, UserDataModel, Guid> userRepository = MockRepository.GenerateMock<IRepository<User, UserDataModel, Guid>>();
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository = MockRepository.GenerateMock<IRepository<Payment, PaymentDataModel, Guid>>();
            IExcelExportService<FineExportModel> excelExportService =
    MockRepository.GenerateMock<IExcelExportService<FineExportModel>>();
            Guid guid1 = new Guid();
            Guid guid2 = new Guid();

            userRepository.Stub(x => x.Find(null)).IgnoreArguments().Return(new User() { Id = guid2 });

            userRepository.Stub(x => x.GetAll()).Return(new List<User>
                                                        {
                                                            new User(){Id = guid2},
                                                            new User()
                                                            {
                                                                Id = guid1,
                                                                Fines = new List<Fine>
                                                                        {
                                                                            new Fine()
                                                                            {
                                                                                Id = new Guid(),
                                                                                AwardedDate = DateTime.Now,
                                                                                ModifiedDate = DateTime.Now,
                                                                                IssuerId = guid2
                                                                            },
                                                                            new Fine()
                                                                            {
                                                                                Id = new Guid(),
                                                                                AwardedDate = DateTime.Now,
                                                                                ModifiedDate = DateTime.Now,
                                                                                IssuerId = guid2
                                                                            },
                                                                            new Fine()
                                                                            {
                                                                                Id = new Guid(),
                                                                                AwardedDate = DateTime.Now,
                                                                                ModifiedDate = DateTime.Now,
                                                                                IssuerId = guid2
                                                                            }
                                                                        }
                                                            }
                                                        });

            FineApi fineApi = new FineApi(userRepository, paymentRepository, new FineMapper(), new UserMapper(new FineMapper()), MockRepository.GenerateMock<IPaymentMapper>(), excelExportService);

            List<FeedFineModel> finesList = fineApi.GetLatestSetOfFines(0, 10);

            Assert.That(finesList.Count == 3);
        }

        [Test]
        public void GivenAListOfRecentPaidAndNewFines_When_RetrievingLatestFinesForFeed_Then_AllShouldBeRetrieved() {
            IRepository<User, UserDataModel, Guid> userRepository = MockRepository.GenerateMock<IRepository<User, UserDataModel, Guid>>();
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository = MockRepository.GenerateMock<IRepository<Payment, PaymentDataModel, Guid>>();
            IExcelExportService<FineExportModel> excelExportService =
    MockRepository.GenerateMock<IExcelExportService<FineExportModel>>();
            Guid paymentId1 = Guid.NewGuid();

            Guid userGuid1 = Guid.NewGuid();
            Guid userGuid2 = Guid.NewGuid();

            userRepository.Stub(x => x.Find(null)).IgnoreArguments().Return(new User() { Id = userGuid2 });

            int count = 0;

            paymentRepository.Stub(x => x.Find(new PaymentSpecification().WithId(paymentId1))).IgnoreArguments().Return(new Payment() {
                Id = paymentId1,
                PaidDate = new DateTime(2015, 09, 24),
                PayerId = userGuid2
            });

            paymentRepository.Stub(x => x.FindAll(null)).IgnoreArguments().Return(
                new List<Payment> {
                    new Payment() 
                    {
                        Id = paymentId1,
                        PaidDate = new DateTime(2015, 09, 25),
                        PayerId = userGuid2
                    }
                }
            );

            userRepository.Stub(x => x.GetAll()).Return(new List<User>
                                                        {
                                                            new User(){Id = userGuid2},
                                                            new User()
                                                            {
                                                                Id = userGuid1,
                                                                Fines = new List<Fine>
                                                                        {
                                                                            new Fine()
                                                                            {
                                                                                Id = new Guid(),
                                                                                AwardedDate = new DateTime(2015,09,20),
                                                                                ModifiedDate = new DateTime(2015,09,21),
                                                                                IssuerId = userGuid2,
                                                                                PaymentId =  paymentId1
                                                                            },
                                                                            new Fine()
                                                                            {
                                                                                Id = new Guid(),
                                                                                AwardedDate = new DateTime(2015,09,22),
                                                                                ModifiedDate = new DateTime(2015,09,23),
                                                                                IssuerId = userGuid2
                                                                            },
                                                                            new Fine()
                                                                            {
                                                                                Id = new Guid(),
                                                                                AwardedDate = new DateTime(2015,09,10),
                                                                                ModifiedDate = new DateTime(2015,09,10),
                                                                                IssuerId = userGuid2
                                                                            }
                                                                        }
                                                            }
                                                        });

            FineApi fineApi = new FineApi(userRepository, paymentRepository, new FineMapper(), new UserMapper(new FineMapper()), MockRepository.GenerateMock<IPaymentMapper>(), excelExportService);

            List<FeedFineModel> finesList = fineApi.GetLatestSetOfFines(0, 10);

            Assert.That(finesList.Count == 4);
        }

        [Test]
        public void GivenALimitedListOfRecentPaidAndNewFines_When_RetrievingLatestFinesForFeed_Then_TheLatestOfBothShouldBeRetrieved() {
            IRepository<User, UserDataModel, Guid> userRepository = MockRepository.GenerateMock<IRepository<User, UserDataModel, Guid>>();
            IRepository<Payment, PaymentDataModel, Guid> paymentRepository = MockRepository.GenerateMock<IRepository<Payment, PaymentDataModel, Guid>>();
            IExcelExportService<FineExportModel> excelExportService =
    MockRepository.GenerateMock<IExcelExportService<FineExportModel>>();
            Guid userGuid1 = Guid.NewGuid();
            Guid userGuid2 = Guid.NewGuid(); 

            Guid fineId1 = Guid.NewGuid(); 
            Guid fineId2 = Guid.NewGuid(); 
            Guid fineId3 = Guid.NewGuid();

            Guid paymentId2 = Guid.NewGuid();

            User user = new User() { Id = userGuid2 }; 
            userRepository.Stub(x => x.Find(null)).IgnoreArguments().Return(user);

            paymentRepository.Stub(x => x.Find(null)).IgnoreArguments().Return(new Payment() {
                Id = paymentId2,
                PaidDate = new DateTime(2015, 09, 24),
                PayerId = userGuid2
            });

            paymentRepository.Stub(x => x.FindAll(null)).IgnoreArguments().Return(new List<Payment> {new Payment() {
                Id = paymentId2,
                PaidDate = new DateTime(2015, 09, 24),
                PayerId = userGuid2
            }});
            
            userRepository.Stub(x => x.GetAll()).Return(new List<User>
                                                        {
                                                            user,
                                                            new User()
                                                            {
                                                                Id = userGuid1,
                                                                Fines = new List<Fine>
                                                                        {
                                                                            new Fine()
                                                                            {
                                                                                Id = fineId1,
                                                                                AwardedDate = new DateTime(2015,09,20),
                                                                                ModifiedDate = new DateTime(2015,09,21),
                                                                                IssuerId = userGuid2
                                                                            },
                                                                            new Fine()
                                                                            {
                                                                                Id = fineId2,
                                                                                AwardedDate = new DateTime(2015,09,22),
                                                                                ModifiedDate = new DateTime(2015,09,23),
                                                                                IssuerId = userGuid2
                                                                            },
                                                                            new Fine()
                                                                            {
                                                                                Id = fineId3,
                                                                                AwardedDate = new DateTime(2015,09,10),
                                                                                ModifiedDate = new DateTime(2015,09,24),
                                                                                IssuerId = userGuid2,
                                                                                PaymentId = paymentId2
                                                                            }
                                                                        }
                                                            }
                                                        });

            FineApi fineApi = new FineApi(userRepository, paymentRepository, new FineMapper(), new UserMapper(new FineMapper()), MockRepository.GenerateMock<IPaymentMapper>(),excelExportService);

            List<FeedFineModel> finesList = fineApi.GetLatestSetOfFines(0, 3);

            Assert.That(finesList.Count == 3);
            Assert.AreEqual(0, finesList.Count(x => x.Id == fineId1));
            Assert.AreEqual(1, finesList.Count(x => x.Id == fineId2));
            Assert.AreEqual(1, finesList.Count(x => x.Id == fineId3));
            Assert.AreEqual(1, finesList.Count(x => x.Id == paymentId2));
            Assert.AreEqual(1, finesList.Count(x => x.IsPaid));
        }
    }
}