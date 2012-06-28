using System.Linq;
using NUnit.Framework;
using Should;

namespace Client.Test
{
    [TestFixture]
    public class TestCommands
    {
        [TestFixtureSetUp]
        public void CreateClient()
        {
            client = new RedisClient(new RedisServer("10.210.32.24"), new ConsoleConnectionLog());
//          client = new RedisClient(new RedisServer("192.168.1.108"), new ConsoleConnectionLog());
        }

        [SetUp]
        public void SetUp()
        {
            var statusReply = client.FlushAll();
            statusReply.Status.ShouldEqual("OK");
        }

        IRedisClient client;

        [Test]
        public void DbSize()
        {
            client.Set("foo", "bar");
            client.Set("foo2", "bar");

            client.DbSize().ShouldEqual(2);
        }
        
        [Test]
        public void Del()
        {
            client.Set("foo", "bar");

            client.Del("foo");

            client.Keys("*").ShouldBeEmpty();
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
        public void Set()
        {
            client.Set("foo", "bar");

            client.Keys("*").Single().ShouldEqual("foo");
        }
    }
}