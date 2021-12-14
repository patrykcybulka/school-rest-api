using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using school_rest_api.Databases;
using school_rest_api.Entries;
using StackExchange.Redis;
using System;

namespace school_rest_api_unit_tests
{
    [TestClass]
    public class RedisDbManagerTest
    {
        [TestMethod]
        public void GetDateAsync_NoData()
        {
            var database = new Mock<IDatabase>();
            database.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);

            var connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            connectionMultiplexer.Setup( c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(database.Object);

            var redisDbManager = new RedisDbManager(connectionMultiplexer.Object);

            var date = redisDbManager.GetDataAsync<object>("object").Result;

            Assert.IsNull(date);
        }

        [TestMethod]
        public void GetDateAsync_DataExist()
        {
            var classEntry = new ClassEntry { Id = Guid.NewGuid(), Name = 'A' };

            var classEntrySerialized = JsonConvert.SerializeObject(classEntry);

            var database = new Mock<IDatabase>();
            database.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(classEntrySerialized);

            var connectionMultiplexer = new Mock<IConnectionMultiplexer>();
            connectionMultiplexer.Setup(c => c.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(database.Object);

            var redisDbManager = new RedisDbManager(connectionMultiplexer.Object);

            var date = redisDbManager.GetDataAsync<ClassEntry>("classEntry").Result;

            Assert.IsNotNull(date);
            Assert.AreEqual(classEntry.Id, date.Id);
            Assert.AreEqual(classEntry.Name, date.Name);
        }
    }
}
