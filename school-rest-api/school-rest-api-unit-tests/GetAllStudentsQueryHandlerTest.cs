using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Functions.Queries;
using school_rest_api_unit_tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class GetAllStudentsQueryHandlerTest
    {
        [TestMethod]
        public void GetAllStudentsQueryHandler_NoDataInRedisCache()
        {
            var studentEntries = new List<StudentEntry>();
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName1", Surname = "StudentSurname1", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName2", Surname = "StudentSurname2", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName3", Surname = "StudentSurname3", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetAllStudent()).Returns(studentEntries);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<StudentEntry>>(It.IsAny<string>())).ReturnsAsync(default(List<StudentEntry>));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<StudentEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getAllStudentsQueryHandler = new GetAllStudentsQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var getAllStudentsQuery = new GetAllStudentsQuery();

            var data = getAllStudentsQueryHandler.Handle(getAllStudentsQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentEntries.Count, data.Result.Students.ToList().Count);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetAllStudentsQueryHandler_DataInRedisCache()
        {
            var studentEntries = new List<StudentEntry>();
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName1", Surname = "StudentSurname1", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName2", Surname = "StudentSurname2", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "StudentFirstName3", Surname = "StudentSurname3", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });

            bool getAllStudentMethodUsed = false;

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetAllStudent()).Returns(studentEntries).Callback(() => getAllStudentMethodUsed = true);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<StudentEntry>>(It.IsAny<string>())).ReturnsAsync(studentEntries);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<StudentEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getAllStudentsQueryHandler = new GetAllStudentsQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var getAllStudentsQuery = new GetAllStudentsQuery();

            var data = getAllStudentsQueryHandler.Handle(getAllStudentsQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentEntries.Count, data.Result.Students.ToList().Count);
            Assert.IsFalse(getAllStudentMethodUsed);
            Assert.IsFalse(setDataAsyncMethodUsed);
        }
    }
}
