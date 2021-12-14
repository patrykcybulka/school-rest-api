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
    public class GetClassByIdQueryHandlerTest
    {
        [TestMethod]
        public void GetClassByIdQueryHandler_RedisNoCache()
        {
            var classEntry = new ClassEntry { Id = Guid.NewGuid(), Name = 'A' };

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetClass(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(classEntry);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<ClassEntry>(It.IsAny<string>())).ReturnsAsync(default(ClassEntry));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<ClassEntry>())).Callback(() => setDataAsyncMethodUsed = true);

            var getClassByIdQueryHandler = new GetClassByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetClassByIdDTO { Id = classEntry.Id };

            var getClassByIdQuery = new GetClassByIdQuery(model);

            var data = getClassByIdQueryHandler.Handle(getClassByIdQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(classEntry.Name, data.Result.Name);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetClassByIdQueryHandler_RedisCache()
        {
            var classEntry = new ClassEntry { Id = Guid.NewGuid(), Name = 'A' };

            bool getClassMethodUsed = false;

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetClass(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(classEntry).Callback(() => getClassMethodUsed = true);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var key = nameof(GetClassByIdQuery) + classEntry.Id.ToString();

            redisDbManagerMock.Setup(r => r.GetDataAsync<ClassEntry>(key)).ReturnsAsync(classEntry);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<ClassEntry>())).Callback(() => setDataAsyncMethodUsed = true);

            var getClassByIdQueryHandler = new GetClassByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetClassByIdDTO { Id = classEntry.Id };

            var getClassByIdQuery = new GetClassByIdQuery(model);

            var data = getClassByIdQueryHandler.Handle(getClassByIdQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(classEntry.Name, data.Result.Name);
            Assert.IsFalse(getClassMethodUsed);
            Assert.IsFalse(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetClassByIdQueryHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetClass(It.IsAny<Expression<Func<ClassEntry, bool>>>())).Returns(default(ClassEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<ClassEntry>(It.IsAny<string>())).ReturnsAsync(default(ClassEntry));

            var getClassByIdQueryHandler = new GetClassByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetClassByIdDTO { Id = Guid.NewGuid() };

            var getClassByIdQuery = new GetClassByIdQuery(model);

            TestUtils.CheckError(() => { return getClassByIdQueryHandler.Handle(getClassByIdQuery, CancellationTokenGenerator.Generate()); }, EErrorCode.ClassNotExist);
        }
    }
}
