namespace Alpha.Main.Site.Tests.Controllers
{
    using System;
    using System.Web.Mvc;
    using Alpha.Main.Site.Controllers;
    using Alpha.Main.Site.Requests.Dashboard.Notice;
    using Alpha.Main.Site.Tests.Constants;
    using Alpha.Main.Site.ViewModels.Dashboard.Notice;
    using Configurations;
    using KellermanSoftware.CompareNetObjects;
    using MediatR;
    using NSubstitute;
    using NUnit.Framework;
    using Sunplus.Agent.Model.Entities;

    [TestFixture]
    public class NoticeControllerTests
    {
        private IMediator mediator;
        private IUserSession userSession;
        private NoticeController noticeController;
        private CompareLogic compareLogic;

        [SetUp]
        public void SetUp()
        {
            mediator = Substitute.For<IMediator>();
            userSession = Substitute.For<IUserSession>();
            compareLogic = new CompareLogic();

            noticeController = new NoticeController(mediator, userSession);
        }

        [Test]
        public void Index_Always_ReturnsViewWithCorrectViewModel()
        {
            // Arrange
            var expectedViewModel = new NoticeViewModel(10);

            // Act
            var result = noticeController.Index() as ViewResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, result.Model).AreEqual);
        }

        [Test]
        public void MarkMessageAsRead_Always_CallsMediatorSendMarkMessageAsReadQuery()
        {
            // Arrange
            userSession.CustId.Returns(CustId.ValidId);
            var messageId = 1;

            // Act 
            noticeController.MarkMessageAsRead(messageId);

            // Assert
            mediator.Received().Send(
                Arg.Is<MarkMessageAsReadQuery>(
                    q => q.CustId == userSession.CustId
                        && q.MessageId == messageId));
        }

        [Test]
        public void MoreMessages_Always_ReturnsCorrectView()
        {
            // Arrange
            var totalMessage = 10;
            var expectedViewName = "Index";

            // Act
            var result = noticeController.MoreMessages(totalMessage) as ViewResult;

            // Assert
            Assert.AreEqual(expectedViewName, result.ViewName);
        }

        [Test]
        public void MoreMessages_Always_ReturnsViewWithCorrectViewModel()
        {
            // Arrange
            var totalMessage = 10;
            var expectedViewModel = new NoticeViewModel(totalMessage);

            // Act
            var result = noticeController.MoreMessages(totalMessage) as ViewResult;

            // Assert
            Assert.True(compareLogic.Compare(expectedViewModel, result.Model).AreEqual);
        }

        [Test]
        public void NoticeMessages_Always_CallsMediatorSendNoticeQuery()
        {
            // Arrange
            userSession.RoleId.Returns(RoleId.Agent);
            var lastPostDate = DateTime.Now;
            var query = new NoticeQuery(userSession, lastPostDate, 10, AppSettings.LocalTest);

            // Act
            noticeController.NoticeMessages(lastPostDate);

            // Assert
            mediator.Received().Send(Arg.Is<NoticeQuery>(p => compareLogic.Compare(p, query).AreEqual));
        }
    }
}
