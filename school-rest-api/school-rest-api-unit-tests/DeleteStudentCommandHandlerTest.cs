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
    public class DeleteStudentCommandHandlerTest
    {
        [TestMethod]
        public void DeleteStudentCommandHandler_StudentNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(default(StudentEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var deleteStudentCommandHandler = new DeleteStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new DeleteStudentDTO
            {
                Id = Guid.NewGuid()
            };

            var deleteStudentCommand = new DeleteStudentCommand(model);

            TestUtils.CheckError(() => { return deleteStudentCommandHandler.Handle(deleteStudentCommand, CancellationTokenGenerator.Generate()); }, EErrorCode.StudentNotExist);
        }

        [TestMethod]
        public void DeleteStudentCommandHandler_Valid()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();

            var studentEntry = new StudentEntry
            {
                Id            = Guid.NewGuid(),
                ClassId       = Guid.NewGuid(),
                FirstName     = "StudentFirstName1",
                Surname       = "StudentSurname1",
                LanguageGroup = (ELanguageGroup)3,
                Gender        = EGender.Male,
            };

            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(studentEntry);

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var deleteStudentCommandHandler = new DeleteStudentCommandHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new DeleteStudentDTO
            {
                Id = Guid.NewGuid()
            };

            var deleteStudentCommand = new DeleteStudentCommand(model);

            var data = deleteStudentCommandHandler.Handle(deleteStudentCommand, CancellationTokenGenerator.Generate());

            Assert.IsNotNull(data);
        }
    }
}
