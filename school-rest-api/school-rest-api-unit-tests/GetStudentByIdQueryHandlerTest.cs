using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;
using System;
using System.Linq.Expressions;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class GetStudentByIdQueryHandlerTest
    {
        [TestMethod]
        public void GetStudentByIdQueryHandler_RedisNoCache()
        {
            var studentEntry = new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName1", Surname = "StudentSumary1", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English };

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(studentEntry);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<StudentEntry>(It.IsAny<string>())).ReturnsAsync(default(StudentEntry));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<StudentEntry>())).Callback(() => setDataAsyncMethodUsed = true);

            var getStudentByIdQueryHandler = new GetStudentByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetStudentByIdDTO { Id = studentEntry.Id };

            var getStudentByIdQuery = new GetStudentByIdQuery(model);

            var data = getStudentByIdQueryHandler.Handle(getStudentByIdQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentEntry.FirstName, data.Result.FirstName);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetStudentByIdQueryHandler_RedisCache()
        {
            var studentEntry = new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName1", Surname = "StudentSumary1", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English };

            bool getStudentMethodUsed = false;

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(studentEntry).Callback(() => getStudentMethodUsed = true);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var key = nameof(GetStudentByIdQuery) + studentEntry.Id.ToString();

            redisDbManagerMock.Setup(r => r.GetDataAsync<StudentEntry>(key)).ReturnsAsync(studentEntry);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<StudentEntry>())).Callback(() => setDataAsyncMethodUsed = true);

            var getStudentByIdQueryHandler = new GetStudentByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetStudentByIdDTO { Id = studentEntry.Id };

            var getStudentByIdQuery = new GetStudentByIdQuery(model);

            var data = getStudentByIdQueryHandler.Handle(getStudentByIdQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentEntry.FirstName, data.Result.FirstName);
            Assert.IsFalse(getStudentMethodUsed);
            Assert.IsFalse(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetStudentByIdQueryHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetStudent(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(default(StudentEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<StudentEntry>(It.IsAny<string>())).ReturnsAsync(default(StudentEntry));

            var getStudentByIdQueryHandler = new GetStudentByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetStudentByIdDTO { Id = Guid.NewGuid() };

            var getStudentByIdQuery = new GetStudentByIdQuery(model);

            TestUtils.CheckError(() => { return getStudentByIdQueryHandler.Handle(getStudentByIdQuery, CancellationTokenGenerator.Generate()); }, EErrorCode.StudentNotExist);
        }
    }
}
