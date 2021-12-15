using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Functions.Commands;
using school_rest_api.Models.DTO;
using school_rest_api_unit_tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class UpdateClassCommandHandlerTest
    {
        [TestMethod]
        public void UpdateClassCommandHandler_NotSupportedClassName()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateClassCommandHandler = new UpdateClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateClassDTO
            {
                Id   = Guid.NewGuid(),
                Name = 'P'
            };

            var UpdateClassCommand = new UpdateClassCommand(model);

            TestUtils.CheckError(() => { return updateClassCommandHandler.Handle(UpdateClassCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.NotSupportedClassName);
        }

        [TestMethod]
        public void UpdateClassCommandHandler_ClassNotExist_Condition1()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetClasses(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(default(List<ClassEntry>));

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateClassCommandHandler = new UpdateClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateClassDTO
            {
                Id = Guid.NewGuid(),
                Name = 'A'
            };

            var UpdateClassCommand = new UpdateClassCommand(model);

            TestUtils.CheckError(() => { return updateClassCommandHandler.Handle(UpdateClassCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }

        [TestMethod]
        public void UpdateClassCommandHandler_NameIsUsed()
        {
            var classId = Guid.NewGuid();

            var classEntries = new List<ClassEntry>
            {
                new ClassEntry
                {
                    Id   = Guid.NewGuid(),
                    Name = 'A'
                },
                new ClassEntry
                {
                    Id   = classId,
                    Name = 'B'
                }
            };

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetClasses(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(classEntries);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateClassCommandHandler = new UpdateClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateClassDTO
            {
                Id = classId,
                Name = 'A'
            };

            var UpdateClassCommand = new UpdateClassCommand(model);

            TestUtils.CheckError(() => { return updateClassCommandHandler.Handle(UpdateClassCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.NameIsUsed);
        }

        [TestMethod]
        public void UpdateClassCommandHandler_Valid()
        {
            var classId = Guid.NewGuid();

            var classEntries = new List<ClassEntry>
            {
                new ClassEntry
                {
                    Id   = classId,
                    Name = 'A'
                }
            };

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetClasses(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(classEntries);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateClassCommandHandler = new UpdateClassCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateClassDTO
            {
                Id = classId,
                Name = 'B'
            };

            var UpdateClassCommand = new UpdateClassCommand(model);

            var data = updateClassCommandHandler.Handle(UpdateClassCommand, CancellationTokenGenerator.Generate());

            Assert.AreEqual(classId, data.Result.Id);
        }
    }
}
