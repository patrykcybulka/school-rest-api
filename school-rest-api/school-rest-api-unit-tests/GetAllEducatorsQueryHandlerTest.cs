using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Functions.Queries;
using school_rest_api_unit_tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class GetAllEducatorsQueryHandlerTest
    {
        [TestMethod]
        public void GetAllEducatorsQueryHandler_NoDataInRedisCache()
        {
            var educatorEntries = new List<EducatorEntry>();
            educatorEntries.Add(new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName1", Surname = "EducatorSurname1" });
            educatorEntries.Add(new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName2", Surname = "EducatorSurname2" });
            educatorEntries.Add(new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName3", Surname = "EducatorSurname3" });

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetAllEducator()).Returns(educatorEntries);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<EducatorEntry>>(It.IsAny<string>())).ReturnsAsync(default(List<EducatorEntry>));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<EducatorEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getAllEducatorsQueryHandler = new GetAllEducatorsQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var getAllEducatorsQuery = new GetAllEducatorsQuery();

            var data = getAllEducatorsQueryHandler.Handle(getAllEducatorsQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(educatorEntries.Count, data.Result.Educators.ToList().Count);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetAllEducatorsQueryHandler_DataInRedisCache()
        {
            var educatorEntries = new List<EducatorEntry>();
            educatorEntries.Add(new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName1", Surname = "EducatorSurname1" });
            educatorEntries.Add(new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName2", Surname = "EducatorSurname2" });
            educatorEntries.Add(new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName3", Surname = "EducatorSurname3" });

            bool getAllEducatorMethodUsed = false;

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetAllEducator()).Returns(educatorEntries).Callback(() => getAllEducatorMethodUsed = true);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<EducatorEntry>>(It.IsAny<string>())).ReturnsAsync(educatorEntries);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<EducatorEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getAllEducatorsQueryHandler = new GetAllEducatorsQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var getAllEducatorsQuery = new GetAllEducatorsQuery();

            var data = getAllEducatorsQueryHandler.Handle(getAllEducatorsQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(educatorEntries.Count, data.Result.Educators.ToList().Count);
            Assert.IsFalse(getAllEducatorMethodUsed);
            Assert.IsFalse(setDataAsyncMethodUsed);
        }
    }
}
