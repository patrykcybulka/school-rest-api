using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using school_rest_api.Databases;
using school_rest_api.Entries;
using school_rest_api.Functions.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class GetAllClassesQueryHandlerTest
    {
        [TestMethod]
        public void GetAllClassesQueryHandler_RedisNoCache()
        {
            var classEntries = new List<ClassEntry>();
            classEntries.Add(new ClassEntry { Id = Guid.NewGuid(), Name = 'A' });
            classEntries.Add(new ClassEntry { Id = Guid.NewGuid(), Name = 'B' });
            classEntries.Add(new ClassEntry { Id = Guid.NewGuid(), Name = 'C' });

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetAllClass()).Returns(classEntries);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<ClassEntry>>(It.IsAny<string>())).ReturnsAsync(default(List<ClassEntry>));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<ClassEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getAllClassesQueryHandler = new GetAllClassesQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var getAllClassesQuery = new GetAllClassesQuery();

            var data = getAllClassesQueryHandler.Handle(getAllClassesQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(classEntries.Count, data.Result.Classes.ToList().Count);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetAllClassesQueryHandler_RedisCache()
        {
            var classEntries = new List<ClassEntry>();
            classEntries.Add(new ClassEntry { Id = Guid.NewGuid(), Name = 'A' });
            classEntries.Add(new ClassEntry { Id = Guid.NewGuid(), Name = 'B' });
            classEntries.Add(new ClassEntry { Id = Guid.NewGuid(), Name = 'C' });

            bool getAllClassMethodUsed = false;

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetAllClass()).Returns(classEntries).Callback(() => getAllClassMethodUsed = true);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<List<ClassEntry>>(It.IsAny<string>())).ReturnsAsync(classEntries);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<IEnumerable<ClassEntry>>())).Callback(() => setDataAsyncMethodUsed = true);

            var getAllClassesQueryHandler = new GetAllClassesQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var getAllClassesQuery = new GetAllClassesQuery();

            var data = getAllClassesQueryHandler.Handle(getAllClassesQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(classEntries.Count, data.Result.Classes.ToList().Count);
            Assert.IsFalse(getAllClassMethodUsed);
            Assert.IsFalse(setDataAsyncMethodUsed);
        }
    }
}
