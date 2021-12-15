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
    public class AddStudentCommandHandlerTest
    {
        [TestMethod]
        public void AddStudentCommandHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(false);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addStudentCommandHandler = new AddStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddStudentDTO
            {
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                Gender        = EGender.Male,
                LanguageGroup = ELanguageGroup.English
            };

            var addStudentCommand = new AddStudentCommand(model);

            TestUtils.CheckError(() => { return addStudentCommandHandler.Handle(addStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }

        [TestMethod]
        public void AddStudentCommandHandler_LanguageGrupeNotExitst()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addStudentCommandHandler = new AddStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddStudentDTO
            {
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                Gender        = EGender.Male,
                LanguageGroup = (ELanguageGroup)3
            };

            var addStudentCommand = new AddStudentCommand(model);

            TestUtils.CheckError(() => { return addStudentCommandHandler.Handle(addStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.LanguageGrupeNotExitst);
        }

        [TestMethod]
        public void AddStudentCommandHandler_UndefinedGender()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addStudentCommandHandler = new AddStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddStudentDTO
            {
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                Gender        = (EGender)3,
                LanguageGroup = ELanguageGroup.English
            };

            var addStudentCommand = new AddStudentCommand(model);

            TestUtils.CheckError(() => { return addStudentCommandHandler.Handle(addStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.UndefinedGender);
        }

        [TestMethod]
        public void AddStudentCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var addStudentCommandHandler = new AddStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new AddStudentDTO
            {
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                Gender        = EGender.Male,
                LanguageGroup = ELanguageGroup.English
            };

            var addStudentCommand = new AddStudentCommand(model);

            var data = addStudentCommandHandler.Handle(addStudentCommand, CancellationTokenGenerator.Generate());

            Assert.IsTrue(data.Result.Id != Guid.Empty);
        }
    }
}
