using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Enums;
using school_rest_api.Functions.Queries;
using school_rest_api.Models.DTO;
using school_rest_api_unit_tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class GetStudentsByClassAndSortedByGenderQueryHandlerTest
    {
        [TestMethod]
        public void GetStudentsByClassAndSortedByGenderQueryHandler_NoDataInRedisCache()
        {
            var classId = Guid.NewGuid();

            var studentEntries = new List<StudentEntry>();
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "StudentFirstName1", Surname = "StudentSurname1", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "StudentFirstName2", Surname = "StudentSurname2", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "StudentFirstName3", Surname = "StudentSurname3", Gender = EGender.Female, LanguageGroup = ELanguageGroup.English });

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetStudents(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(studentEntries);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<StudentEntry>>(It.IsAny<string>())).ReturnsAsync(default(List<StudentEntry>));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<StudentEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getStudentsByClassAndSortedByGenderQueryHandler = new GetStudentsByClassAndSortedByGenderQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetStudentsByClassAndSortedByGenderDTO
            {
                Id = classId,
                Gender = EGender.Female
            };

            var getStudentsByClassAndSortedByGenderQuery = new GetStudentsByClassAndSortedByGenderQuery(model);

            var data = getStudentsByClassAndSortedByGenderQueryHandler.Handle(getStudentsByClassAndSortedByGenderQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentEntries.Last().Id, data.Result.Students.First().Id);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetStudentsByClassAndSortedByGenderQueryHandler_DataInRedisCache()
        {
            var classId = Guid.NewGuid();

            var studentEntries = new List<StudentEntry>();
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "StudentFirstName1", Surname = "StudentSurname1", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "StudentFirstName2", Surname = "StudentSurname2", Gender = EGender.Male, LanguageGroup = ELanguageGroup.English });
            studentEntries.Add(new StudentEntry { Id = Guid.NewGuid(), ClassId = classId, FirstName = "StudentFirstName3", Surname = "StudentSurname3", Gender = EGender.Female, LanguageGroup = ELanguageGroup.English });

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetStudents(It.IsAny<Expression<Func<StudentEntry, bool>>>())).Returns(studentEntries);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<StudentEntry>>(It.IsAny<string>())).ReturnsAsync(studentEntries);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<StudentEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getStudentsByClassAndSortedByGenderQueryHandler = new GetStudentsByClassAndSortedByGenderQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetStudentsByClassAndSortedByGenderDTO
            {
                Id = classId,
                Gender = EGender.Female
            };

            var getStudentsByClassAndSortedByGenderQuery = new GetStudentsByClassAndSortedByGenderQuery(model);

            var data = getStudentsByClassAndSortedByGenderQueryHandler.Handle(getStudentsByClassAndSortedByGenderQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(studentEntries.Count, data.Result.Students.Count());
            Assert.IsFalse(setDataAsyncMethodUsed);
        }
    }
}
