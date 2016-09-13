using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiniRedis.Tests
{
    [TestClass]
    public class MemoryStorageTest
    {
        [TestMethod]
        public void SetExTest_HasExpired()
        {
            MemoryStorage memoryStorage = new MemoryStorage();
            const string key = "test_SetTest_WithOutExist";
            const string originalValue = "SetTest_WithOutExist";
            const int timeToExpire = 2;

            memoryStorage.Set(key, originalValue, timeToExpire);
            Thread.Sleep((timeToExpire + 1 /* Add one second */) * 1000);

            object expected = memoryStorage.Get(key);
            Assert.IsNull(expected);
        }

        [TestMethod]
        public void Del_WithNotExist()
        {
            MemoryStorage memoryStorage = new MemoryStorage();

            const int maxElements = 20;

            var keys = new HashSet<string>();

            for (int i = 0; i < maxElements; i++)
            {
                string key = "test_SetTest_WithOutExist" + i;
                string value = "SetTest_WithOutExist" + i;
                keys.Add(key);

                memoryStorage.Set(key, value);
            }
            Assert.IsTrue(memoryStorage.Dbsize() == maxElements);

            keys.Add("test_SetTest_not_exist_element");
            memoryStorage.Del(keys.ToArray());

            Assert.IsTrue(memoryStorage.Dbsize() == 0);
        }

        [TestMethod]
        public void Incr_Without_Element()
        {
            MemoryStorage memoryStorage = new MemoryStorage();
            const string key = "test_SetTest_WithOutExist";

            memoryStorage.Incr(key);

            object val = memoryStorage.Get(key);

            Assert.AreEqual(val, 1);
        }

        [TestMethod]
        public void Incr_With_Element()
        {
            MemoryStorage memoryStorage = new MemoryStorage();
            const string key = "test_SetTest_WithOutExist";
            const int originalValue = 10;

            memoryStorage.Set(key, originalValue);
            memoryStorage.Incr(key);

            object val = memoryStorage.Get(key);

            Assert.AreEqual(val, originalValue + 1);
        }

        [TestMethod]
        public void SetTest_WithOutExist()
        {
            MemoryStorage memoryStorage = new MemoryStorage();
            const string key = "test_SetTest_WithOutExist";
            const string originalValue = "SetTest_WithOutExist";

            memoryStorage.Set(key, originalValue);

            Assert.AreEqual(memoryStorage.Get(key), originalValue);
        }

        [TestMethod]
        public void SetTest_WithExist()
        {
            MemoryStorage memoryStorage = new MemoryStorage();

            const string key = "test_SetTest_WithOutExist";
            const string originalValue = "SetTest_WithOutExist";

            memoryStorage.Set(key, originalValue);
            Assert.AreEqual(memoryStorage.Get(key), originalValue);

            memoryStorage.Set(key, originalValue + originalValue);
            Assert.AreNotEqual(memoryStorage.Get(key), originalValue);
        }
    }
}
