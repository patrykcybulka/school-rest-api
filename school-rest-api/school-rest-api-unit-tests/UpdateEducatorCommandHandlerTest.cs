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
    public class UpdateEducatorCommandHandlerTest
    {
        [TestMethod]
        public void UpdateEducatorCommandHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(false);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateEducatorCommandHandler = new UpdateEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateEducatorDTO
            {
                Id        = Guid.NewGuid(),
                ClassId   = Guid.NewGuid(),
                FirstName = "EducatorFirstName1",
                Surname   = "EducatorSurname1"
            };

            var updateEducatorCommand = new UpdateEducatorCommand(model);

            TestUtils.CheckError(() => { return updateEducatorCommandHandler.Handle(updateEducatorCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }

        [TestMethod]
        public void UpdateEducatorCommandHandler_EducatorNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);
            schoolDbManagerMock.Setup(s => s.GetEducators(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(default(List<EducatorEntry>));

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateEducatorCommandHandler = new UpdateEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateEducatorDTO
            {
                Id        = Guid.NewGuid(),
                ClassId   = Guid.NewGuid(),
                FirstName = "EducatorFirstName1",
                Surname   = "EducatorSurname1"
            };

            var updateEducatorCommand = new UpdateEducatorCommand(model);

            TestUtils.CheckError(() => { return updateEducatorCommandHandler.Handle(updateEducatorCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.EducatorNotExist);
        }

        [TestMethod]
        public void UpdateEducatorCommandHandler_ClassAssignedToAnotherEducator_Condition1()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var classId = Guid.NewGuid();

            var educatorEntries = new List<EducatorEntry> { new EducatorEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "EducatorFirstName1", Surname = "EducatorSurname1" } };

            schoolDbManagerMock.Setup(s => s.GetEducators(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(educatorEntries);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateEducatorCommandHandler = new UpdateEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateEducatorDTO
            {
                Id        = Guid.NewGuid(),
                ClassId   = classId,
                FirstName = "EducatorFirstName2",
                Surname   = "EducatorSurname2"
            };

            var updateEducatorCommand = new UpdateEducatorCommand(model);

            TestUtils.CheckError(() => { return updateEducatorCommandHandler.Handle(updateEducatorCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassAssignedToAnotherEducator);
        }

        [TestMethod]
        public void UpdateEducatorCommandHandler_ClassAssignedToAnotherEducator_Condition2()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var educatorId = Guid.NewGuid();
            var classId = Guid.NewGuid();

            var educatorEntries = new List<EducatorEntry> {
                new EducatorEntry
                { 
                    Id        = Guid.NewGuid(),
                    ClassId   = classId,
                    FirstName = "EducatorFirstName1",
                    Surname   = "EducatorSurname1" },
                new EducatorEntry
                {
                    Id        = educatorId,
                    ClassId   = Guid.NewGuid(),
                    FirstName = "EducatorFirstName2",
                    Surname   = "EducatorSurname2" },
            };

            schoolDbManagerMock.Setup(s => s.GetEducators(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(educatorEntries);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateEducatorCommandHandler = new UpdateEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateEducatorDTO
            {
                Id        = educatorId,
                ClassId   = classId,
                FirstName = "EducatorFirstName3",
                Surname   = "EducatorSurname3"
            };

            var updateEducatorCommand = new UpdateEducatorCommand(model);

            TestUtils.CheckError(() => { return updateEducatorCommandHandler.Handle(updateEducatorCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassAssignedToAnotherEducator);
        }

        [TestMethod]
        public void UpdateEducatorCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var educatorId = Guid.NewGuid();
            var classId = Guid.NewGuid();

            var educatorEntries = new List<EducatorEntry> { new EducatorEntry
            {
                Id        = educatorId,
                ClassId   = Guid.NewGuid(),
                FirstName = "EducatorFirstName2",
                Surname   = "EducatorSurname2" },
            };

            schoolDbManagerMock.Setup(s => s.GetEducators(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(educatorEntries);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateEducatorCommandHandler = new UpdateEducatorCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateEducatorDTO
            {
                Id = educatorId,
                ClassId = classId,
                FirstName = "EducatorFirstName3",
                Surname = "EducatorSurname3"
            };

            var updateEducatorCommand = new UpdateEducatorCommand(model);

            var data = updateEducatorCommandHandler.Handle(updateEducatorCommand, CancellationTokenGenerator.Generate());

            Assert.AreEqual(model.Id, data.Result.Id);
        }
    }
}
