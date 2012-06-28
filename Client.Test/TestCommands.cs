using System;
using System.Linq;
using Client.Replies;
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

        [Test]
        public void Exists()
        {
            client.Exists("foo").ShouldBeFalse();

            client.Set("foo", "bar");

            client.Exists("foo").ShouldBeTrue();
        }

        [Test]
        public void Expires()
        {
            client.Set("foo", "bar");

            client.Expire("foo", 60).ShouldBeTrue();
        }

        [Test]
        public void Ttl()
        {
            client.Set("foo", "bar");

            client.Expire("foo", 6000);

            client.Ttl("foo").ShouldBeInRange(1, 6000);
        }

        [Test]
        public void ExpireAt()
        {
            client.Set("foo", "bar");

            client.ExpireAt("foo", DateTime.UtcNow.AddSeconds(200));

            Assert.That(client.Ttl("foo"), Is.GreaterThan(1));
        }

        [Test, ExpectedException(typeof(RedisReplyException), ExpectedMessage = "ERR Client sent AUTH, but no password is set")]
        public void Auth()
        {
            client.Auth("foo");
        }

        [Test]
        public void Echo()
        {
            client.Echo("message").ShouldEqual("message");
        }

        [Test]
        public void Ping()
        {
            client.Ping().Status.ShouldEqual("PONG");
        }

        [Test]
        public void Quit()
        {
            client.Quit().Status.ShouldEqual("OK");
            CreateClient();
        }

        [Test]
        public void Select()
        {
            client.Select(0).Status.ShouldEqual("OK");
        }

        [Test]
        public void Info()
        {
            client.Info()["role"].ShouldEqual("master");
        }
    }
}