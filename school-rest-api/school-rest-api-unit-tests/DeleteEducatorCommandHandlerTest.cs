using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Functions.Commands;
using school_rest_api.Models.DTO;
using school_rest_api_unit_tests.Utils;
using System;
using System.Linq.Expressions;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class DeleteEducatorCommandHandlerTest
    {
        [TestMethod]
        public void DeleteEducatorCommandHandler_EducatorNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.GetEducator(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(default(EducatorEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var deleteEducatorCommandHandler = new DeleteEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new DeleteEducatorDTO
            {
                Id = Guid.NewGuid()
            };

            var deleteEducatorCommand = new DeleteEducatorCommand(model);

            TestUtils.CheckError(() => { return deleteEducatorCommandHandler.Handle(deleteEducatorCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.EducatorNotExist);
        }

        [TestMethod]
        public void DeleteEducatorCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            var educatorEntry = new EducatorEntry
            {
                Id        = Guid.NewGuid(),
                ClassId   = Guid.NewGuid(),
                FirstName = "EducatorFirstName1",
                Surname   = "EducatorSurname1"
            };

            schoolDbManagerMock.Setup(s => s.GetEducator(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(educatorEntry);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var deleteEducatorCommandHandler = new DeleteEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new DeleteEducatorDTO
            {
                Id = Guid.NewGuid()
            };

            var deleteEducatorCommand = new DeleteEducatorCommand(model);

            var data = deleteEducatorCommandHandler.Handle(deleteEducatorCommand, CancellationTokenGenerator.Generate());

            Assert.IsNotNull(data);
        }
    }
}
