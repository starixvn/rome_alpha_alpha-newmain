namespace Alpha.Main.Tests.Services
{
    using Alpha.Main.Helpers.Impl;
    using Alpha.Main.Models;
    using Alpha.Main.ProfilePicture.Commands;
    using Alpha.Main.ProfilePicture.Models;
    using Alpha.Main.ProfilePicture.Params;
    using Alpha.Main.ProfilePicture.Services;
    using Constants;
    using Fanex.Data.Repository;
    using KellermanSoftware.CompareNetObjects;
    using NSubstitute;
    using NUnit.Framework;
    using System;
    using TestData.Helper;

    [TestFixture]
    public class ProfilePictureServicesTests
    {
        private CompareLogic compareLogic;
        private IDynamicRepository dynamicRepository;
        private ProfilePictureServicesImpl profilePictureServices;
        private ExceptionHandlerWrapper exceptionHandlerWrapper;

        [SetUp]
        public void SetUp()
        {
            compareLogic = new CompareLogic();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            exceptionHandlerWrapper = Substitute.For<ExceptionHandlerWrapper>();
            profilePictureServices = new ProfilePictureServicesImpl(dynamicRepository, exceptionHandlerWrapper);
        }

        [Test]
        public void GetActorProfilePicture_InvalidCustId_ReturnDefaultActorProfilePictureModel()
        {
            var expectedResult = new ActorProfilePictureModel();
            var result = profilePictureServices.GetActorProfilePicture(CustId.InvalidId);

            Assert.True(compareLogic.Compare(result, expectedResult).AreEqual);
        }

        [Test]
        public void GetActorProfilePicture_ValidCustId_ReturnctorProfilePicture()
        {
            var expectedResult = FakeDataHelper.Create<ActorProfilePictureModel>();
            dynamicRepository.Get<ActorProfilePictureModel>(Arg.Any<ICriteria>()).Returns(expectedResult);

            var actualResult = profilePictureServices.GetActorProfilePicture(CustId.ValidId);

            Assert.AreEqual(actualResult, expectedResult);
        }

        [Test]
        public void GetActorProfilePicture_ThrowException_ReturnDefaultActorProfilePictureModel()
        {
            var expectedResult = new ActorProfilePictureModel();
            dynamicRepository.Get<ActorProfilePictureModel>(Arg.Any<ICriteria>()).Returns(ex => throw(new Exception("DB error")));

            var actualResult = profilePictureServices.GetActorProfilePicture(CustId.ValidId);

            Assert.True(compareLogic.Compare(actualResult, expectedResult).AreEqual);
        }

        [Test]
        public void GetActorProfilePicture_ThrowException_CallWriteLog()
        {
            var expectedResult = new ActorProfilePictureModel();
            dynamicRepository.Get<ActorProfilePictureModel>(Arg.Any<ICriteria>()).Returns(ex => throw (new Exception("DB error")));

            var actualResult = profilePictureServices.GetActorProfilePicture(CustId.ValidId);

            exceptionHandlerWrapper.Received().Handle(Arg.Any<Exception>());
        }

        [TestCase(0, "Update Successfully")]
        [TestCase(1, "DB errore")]
        public void UpdateActorProfilePicture_ValidCustId_ReturnResult(int code, string message)
        {
            var expectedResult = new Result() { Code = code, Message = message };

            dynamicRepository
               .When(repo => repo.Execute(Arg.Any<UpdateActorProfilePictureMainServerCommand>()))
               .Do((callInfo) =>
               {
                   var command = callInfo[0] as UpdateActorProfilePictureMainServerCommand;
                   command.ErrCode = expectedResult.Code;
                   command.ErrMsg = expectedResult.Message;
               });

            var result = profilePictureServices.UpdateActorProfilePicture(
                new UpdateActorProfilePictureParam(CustId.InvalidId, 1, "192.168.0.1"));

            Assert.True(compareLogic.Compare(result, expectedResult).AreEqual);
        }

        [Test]
        public void UpdateActorProfilePicture_ThrowException_ReturnDefaultErrorResult()
        {
            var expectedResult = new Result() { Code = 100, Message = "Auto insert Profile Picture Failed!" };

            dynamicRepository
               .When(repo => repo.Execute(Arg.Any<UpdateActorProfilePictureMainServerCommand>()))
               .Do((callInfo) =>
               {
                   throw new Exception("DB eroor");
               });

            var result = profilePictureServices.UpdateActorProfilePicture(
                new UpdateActorProfilePictureParam(CustId.InvalidId, 1, "192.168.0.1"));

            Assert.True(compareLogic.Compare(result, expectedResult).AreEqual);
        }

        [Test]
        public void UpdateActorProfilePicture_ThrowException_CallWriteLog()
        {
            dynamicRepository
               .When(repo => repo.Execute(Arg.Any<UpdateActorProfilePictureMainServerCommand>()))
               .Do((callInfo) =>
               {
                   throw new Exception("DB eroor");
               });

            profilePictureServices.UpdateActorProfilePicture(
                new UpdateActorProfilePictureParam(CustId.InvalidId, 1, "192.168.0.1"));

            exceptionHandlerWrapper.Received().Handle(Arg.Any<Exception>());
        }

        [Test]
        public void UpdateActorProfilePictureStandbyServer_ThrowException_CallWriteLog()
        {
            dynamicRepository
               .When(repo => repo.Execute(Arg.Any<UpdateActorProfilePictureStandbyServerCommand>()))
               .Do((callInfo) =>
               {
                   throw new Exception("DB eroor");
               });

            profilePictureServices.UpdateActorProfilePictureStandbyServer(
                new UpdateActorProfilePictureParam(CustId.InvalidId, 1, "192.168.0.1"));

            exceptionHandlerWrapper.Received().Handle(Arg.Any<Exception>());
        }
    }
}