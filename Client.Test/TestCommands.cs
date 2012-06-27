using System.Linq;
using NUnit.Framework;
using Should;

namespace Client.Test
{
    [TestFixture, Explicit]
    public class TestCommands
    {
        [SetUp]
        public void SetUp()
        {
            client = new RedisClient(new RedisServer("10.210.32.24"), new ConsoleConnectionLog());
            client.FlushAll();
        }

        IRedisClient client;

        [Test]
        public void Keys()
        {
            client.Set("foo", "1");
            client.Set("foo2", "2");
            client.Set("foo3", "3");

            var allKeys = client.Keys("*");

            allKeys.ShouldContain("foo");
            allKeys.ShouldContain("foo2");
            allKeys.ShouldContain("foo3");
        }


        [Test]
        public void Del()
        {
            client.Set("foo", "bar");

            client.Del("foo");

            client.Keys("*").ShouldBeEmpty();
        }

        [Test]
        public void Set()
        {
            client.Set("foo", "bar");

            client.Keys("*").Single().ShouldEqual("foo");
        }

        [Test]
        public void Get()
        {
            client.Set("foo", "bar");

            client.GetString("foo").ShouldEqual("bar");
        }

        [Test]
        public void GetShouldReturnNullIfNotExist()
        {
            client.GetString("foo").ShouldBeNull();
        }
    }
}