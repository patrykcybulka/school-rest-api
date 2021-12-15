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
    public class UpdateStudentCommandHandlerTest
    {
        [TestMethod]
        public void UpdateStudentCommandHandler_LanguageGrupeNotExitst()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            var redisDbManagerMock  = new Mock<IRedisDbManager>();

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateStudentDTO
            {
                Id            = Guid.NewGuid(),
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                LanguageGroup = (ELanguageGroup)3,
                Gender        = EGender.Male,
                
            };

            var updateStudentCommand = new UpdateStudentCommand(model);

            TestUtils.CheckError(() => { return updateStudentCommandHandler.Handle(updateStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.LanguageGrupeNotExitst);
        }

        [TestMethod]
        public void UpdateStudentCommandHandler_UndefinedGender()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            var redisDbManagerMock  = new Mock<IRedisDbManager>();

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateStudentDTO
            {
                Id            = Guid.NewGuid(),
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                LanguageGroup = ELanguageGroup.English,
                Gender        = (EGender)3,

            };

            var updateStudentCommand = new UpdateStudentCommand(model);

            TestUtils.CheckError(() => { return updateStudentCommandHandler.Handle(updateStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.UndefinedGender);
        }

        [TestMethod]
        public void UpdateStudentCommandHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(false);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateStudentDTO
            {
                Id            = Guid.NewGuid(),
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                LanguageGroup = ELanguageGroup.English,
                Gender        = EGender.Male,
            };

            var updateStudentCommand = new UpdateStudentCommand(model);

            TestUtils.CheckError(() => { return updateStudentCommandHandler.Handle(updateStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }

        [TestMethod]
        public void UpdateStudentCommandHandler_StudentNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);
            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(default(StudentEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateStudentDTO
            {
                Id            = Guid.NewGuid(),
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                LanguageGroup = ELanguageGroup.English,
                Gender        = EGender.Male,
            };

            var updateStudentCommand = new UpdateStudentCommand(model);

            TestUtils.CheckError(() => { return updateStudentCommandHandler.Handle(updateStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.StudentNotExist);
        }

        [TestMethod]
        public void UpdateStudentCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.ClassExist(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(true);

            var studentId = Guid.NewGuid();

            var studentEntry = new StudentEntry
            {
                Id            = studentId,
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                LanguageGroup = ELanguageGroup.English,
                Gender        = EGender.Male,
            };

            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(studentEntry);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var updateStudentCommandHandler = new UpdateStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new UpdateStudentDTO
            {
                Id            = studentId,
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName2",
                Surname       = "StudentSurname2",
                LanguageGroup = ELanguageGroup.German,
                Gender        = EGender.Female,
            };

            var updateStudentCommand = new UpdateStudentCommand(model);

            var data = updateStudentCommandHandler.Handle(updateStudentCommand, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentId, data.Result.Id);
        }
    }
}
