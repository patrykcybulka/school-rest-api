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
    public class AddEducatorCommandHandlerTest
    {
        [TestMethod]
        public void AddEducatorCommandHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(false);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addEducatorCommandHandler = new AddEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddEducatorDTO
            {
                ClassId = Guid.NewGuid(),
                FirstName = "EducatorFirstName1",
                Surname = "EducatorSurname1"
            };

            var addEducatorCommand = new AddEducatorCommand(model);

            TestUtils.CheckError(() => { return addEducatorCommandHandler.Handle(addEducatorCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }

        [TestMethod]
        public void AddEducatorCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addEducatorCommandHandler = new AddEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddEducatorDTO
            {
                ClassId = Guid.NewGuid(),
                FirstName = "EducatorFirstName1",
                Surname = "EducatorSurname1"
            };

            var addEducatorCommand = new AddEducatorCommand(model);

            var data = addEducatorCommandHandler.Handle(addEducatorCommand, CancellationTokenGenerator.Generate());

            Assert.IsTrue(data.Result.Id != Guid.Empty);
        }
    }
}
