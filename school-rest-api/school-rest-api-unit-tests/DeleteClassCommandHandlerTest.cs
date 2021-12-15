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
    public class DeleteClassCommandHandlerTest
    {
        [TestMethod]
        public void DeleteClassCommandHandler_StudentNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.GetClass(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(default(ClassEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var deleteClassCommandHandler = new DeleteClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new DeleteClassDTO
            {
                Id = Guid.NewGuid()
            };

            var deleteClassCommand = new DeleteClassCommand(model);

            TestUtils.CheckError(() => { return deleteClassCommandHandler.Handle(deleteClassCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }

        [TestMethod]
        public void DeleteClassCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            var classEntry = new ClassEntry { Id = Guid.NewGuid(), Name = 'A' };

            schoolDbManagerMock.Setup(s => s.GetClass(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(classEntry);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var deleteClassCommandHandler = new DeleteClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new DeleteClassDTO
            {
                Id = Guid.NewGuid()
            };

            var deleteClassCommand = new DeleteClassCommand(model);

            var data = deleteClassCommandHandler.Handle(deleteClassCommand, CancellationTokenGenerator.Generate());

            Assert.IsNotNull(data);
        }
    }
}
