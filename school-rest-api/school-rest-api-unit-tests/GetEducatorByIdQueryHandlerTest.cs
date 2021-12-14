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
    public class GetEducatorByIdQueryHandlerTest
    {
        [TestMethod]
        public void GetEducatorByIdQueryHandler_RedisNoCache()
        {
            var educatorEntry = new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName1", Surname = "EducatorSumary1" };

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetEducator(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(educatorEntry);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<EducatorEntry>(It.IsAny<string>())).ReturnsAsync(default(EducatorEntry));
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<EducatorEntry>())).Callback(() => setDataAsyncMethodUsed = true);

            var getEducatorByIdQueryHandler = new GetEducatorByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetEducatorByIdDTO { Id = educatorEntry.Id };

            var getEducatorByIdQuery = new GetEducatorByIdQuery(model);

            var data = getEducatorByIdQueryHandler.Handle(getEducatorByIdQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(educatorEntry.FirstName, data.Result.FirstName);
            Assert.IsTrue(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetEducatorByIdQueryHandler_RedisCache()
        {
            var educatorEntry = new EducatorEntry { Id = Guid.NewGuid(), ClassId = Guid.NewGuid(), FirstName = "EducatorFirstName1", Surname = "EducatorSumary1" };

            bool getEducatorMethodUsed = false;

            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetEducator(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(educatorEntry).Callback(() => getEducatorMethodUsed = true);

            bool setDataAsyncMethodUsed = false;

            var redisDbManagerMock = new Mock<IRedisDbManager>();

            var key = nameof(GetEducatorByIdQuery) + educatorEntry.Id.ToString();

            redisDbManagerMock.Setup(r => r.GetDataAsync<EducatorEntry>(key)).ReturnsAsync(educatorEntry);
            redisDbManagerMock.Setup(r => r.SetDataAsync(It.IsAny<string>(), It.IsAny<EducatorEntry>())).Callback(() => setDataAsyncMethodUsed = true);

            var getEducatorByIdQueryHandler = new GetEducatorByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetEducatorByIdDTO { Id = educatorEntry.Id };

            var getEducatorByIdQuery = new GetEducatorByIdQuery(model);

            var data = getEducatorByIdQueryHandler.Handle(getEducatorByIdQuery, CancellationTokenGenerator.Generate());

            Assert.AreEqual(educatorEntry.FirstName, data.Result.FirstName);
            Assert.IsFalse(getEducatorMethodUsed);
            Assert.IsFalse(setDataAsyncMethodUsed);
        }

        [TestMethod]
        public void GetEducatorByIdQueryHandler_ClassNotExist()
        {
            var schoolDbManagerMock = new Mock<ISchoolDbManager>();
            schoolDbManagerMock.Setup(s => s.GetEducator(It.IsAny<Expression<Func<EducatorEntry, bool>>>())).Returns(default(EducatorEntry));

            var redisDbManagerMock = new Mock<IRedisDbManager>();
            redisDbManagerMock.Setup(r => r.GetDataAsync<EducatorEntry>(It.IsAny<string>())).ReturnsAsync(default(EducatorEntry));

            var getEducatorByIdQueryHandler = new GetEducatorByIdQueryHandler(schoolDbManagerMock.Object, redisDbManagerMock.Object);

            var model = new GetEducatorByIdDTO { Id = Guid.NewGuid() };

            var getClassByIdQuery = new GetEducatorByIdQuery(model);

            TestUtils.CheckError(() => { return getEducatorByIdQueryHandler.Handle(getClassByIdQuery, CancellationTokenGenerator.Generate()); }, EErrorCode.EducatorNotExist);
        }
    }
}
