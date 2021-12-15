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
    public class AddClassCommandHandlerTest
    {
        [TestMethod]
        public void AddClassCommandHandler_NotSupportedClassName()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addClassCommandHandler = new AddClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddClassDTO
            {
                Name = 'P'
            };

            var addClassCommand = new AddClassCommand(model);

            TestUtils.CheckError(() => { return addClassCommandHandler.Handle(addClassCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.NotSupportedClassName);
        }

        [TestMethod]
        public void AddClassCommandHandler_ClassAlreadyExists()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addClassCommandHandler = new AddClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddClassDTO
            {
                Name = 'A'
            };

            var addClassCommand = new AddClassCommand(model);

            TestUtils.CheckError(() => { return addClassCommandHandler.Handle(addClassCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassAlreadyExists);
        }

        [TestMethod]
        public void AddClassCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(false);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addClassCommandHandler = new AddClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddClassDTO
            {
                Name = 'A'
            };

            var addClassCommand = new AddClassCommand(model);

            var data = addClassCommandHandler.Handle(addClassCommand, CancellationTokenGenerator.Generate());

            Assert.IsTrue(data.Result.Id != Guid.Empty);
        }
    }
}
