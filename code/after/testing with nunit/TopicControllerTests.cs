using mocking_ef_datacontext.Controllers;
using mocking_ef_datacontext.DB;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;

namespace testing_with_nunit
{
    [TestFixture]
    public class TopicControllerTests
    {
        Mock<ITopicsDataContext> _topicsDataContext;

        [SetUp]
        public void SetUp()
        {
            _topicsDataContext = new Mock<ITopicsDataContext>();
        }

        [Test]
        public void TopicController_Get_WhenTopicsCollectionIsInitializedWith2Topics_WillReturn2Topics()
        {
            var topics = new FakeDbSet<Topic>(new List<Topic>
                {
                    new Topic {Name = "ASP.NET Core", Id = 1 },
                    new Topic {Name = "Docker for .NET Developers", Id = 2 }
                });

            _topicsDataContext.SetupGet(g => g.Topics).Returns(topics);
            var controller = new TopicController(_topicsDataContext.Object);
            SetUpHttpRequestParameters(controller);
            var response = controller.Get();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            IEnumerable<dynamic> resultData;
            Assert.IsTrue(response.TryGetContentValue(out resultData));
            var topicsArray = resultData.ToArray();
            var expandoDict0 = (IDictionary<string, object>)topicsArray[0];
            Assert.AreEqual("ASP.NET Core", expandoDict0["name"]);
            Assert.AreEqual(1, expandoDict0["id"]);
            var expandoDict1 = (IDictionary<string, object>)topicsArray[1];
            Assert.AreEqual("Docker for .NET Developers", expandoDict1["name"]);
            Assert.AreEqual(2, expandoDict1["id"]);
        }

        [Test]
        public void TopicController_Get_WhenTheIdRequestedExists_WillReturnATopic()
        {
            var topics = new FakeDbSet<Topic>(new List<Topic>
                {
                    new Topic {Name = "ASP.NET Core", Id = 1 },
                    new Topic {Name = "Docker for .NET Developers", Id = 2 }
                });

            _topicsDataContext.SetupGet(g => g.Topics).Returns(topics);
            var controller = new TopicController(_topicsDataContext.Object);
            SetUpHttpRequestParameters(controller);
            var response = controller.Get(1);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ExpandoObject expando;
            Assert.IsTrue(response.TryGetContentValue(out expando));
            var expandoDict = (IDictionary<string, object>)expando;
            Assert.AreEqual("ASP.NET Core", expandoDict["name"]);
            Assert.AreEqual(1, expandoDict["id"]);
        }


        [Test]
        public void TopicController_Get_WhenTheIdRequestedDoesNotExists_WillReturnA404()
        {
            var topics = new FakeDbSet<Topic>(new List<Topic> {
                    new Topic {Name = "ASP.NET Core", Id = 1 },
                    new Topic {Name = "Docker for .NET Developers", Id = 2 }
                });
            _topicsDataContext.SetupGet(g => g.Topics).Returns(topics);
            var controller = new TopicController(_topicsDataContext.Object);
            SetUpHttpRequestParameters(controller);
            var response = controller.Get(3);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            ExpandoObject expando;
            Assert.IsTrue(response.TryGetContentValue(out expando));
            var expandoDict = (IDictionary<string, object>)expando;
            Assert.AreEqual("The topic you requested was not found", expandoDict["message"]);
        }


        [Test]
        public void TopicController_Get_WhenTheIdAndNameRequestedExists_WillReturnATopic()
        {
            var topics = new FakeDbSet<Topic>(new List<Topic>
                {
                    new Topic {Name = "ASP.NET Core", Id = 1, Tutorials = new List<Tutorial>
                    {
                        new Tutorial
                        {
                            Name = "ASP.NET Core on Ubuntu",
                            Type = "video",
                            Url = "http://www.learninghabits.co.za/#/topics/ubuntu"
                        }
                    }},
                    new Topic {Name = "Docker for .NET Developers", Id = 2 }
                });

            _topicsDataContext.SetupGet(g => g.Topics).Returns(topics);
            var controller = new TopicController(_topicsDataContext.Object);
            SetUpHttpRequestParameters(controller);
            var response = controller.Get(1, "ASP.NET Core on Ubuntu");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ExpandoObject expando;
            Assert.IsTrue(response.TryGetContentValue(out expando));
            var expandoDict = (IDictionary<string, object>)expando;
            Assert.AreEqual(1, expandoDict["id"]);
            var tutorials = expandoDict["tutorials"] as IEnumerable<dynamic>;
            Assert.IsNotNull(tutorials);
            var tutorialArray = tutorials.ToArray();
            Assert.AreEqual(1, tutorialArray.Length);
            Assert.AreEqual("ASP.NET Core on Ubuntu", ((IDictionary<string, object>)tutorialArray[0])["name"]);
        }

        [Test]
        public void TopicController_Get_WhenTheIdOrNameRequestedDoesNotExists_WillReturnA404()
        {
            var topics = new FakeDbSet<Topic>(new List<Topic> {
                    new Topic {Name = "ASP.NET Core", Id = 1 },
                    new Topic {Name = "Docker for .NET Developers", Id = 2 }
                });
            _topicsDataContext.SetupGet(g => g.Topics).Returns(topics);
            var controller = new TopicController(_topicsDataContext.Object);
            SetUpHttpRequestParameters(controller);
            var response = controller.Get(3, "Some Random Name");
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            ExpandoObject expando;
            Assert.IsTrue(response.TryGetContentValue(out expando));
            var expandoDict = (IDictionary<string, object>)expando;
            Assert.AreEqual("The tutorial  you requested was not found", expandoDict["message"]);
        }

        [Test]
        public void TopicController_Post_WhenTheTopicIsAddedSuccessfully_WillReturnANavigationProperty()
        {
            var topics = new FakeDbSet<Topic>(new List<Topic> { });
            _topicsDataContext.SetupGet(g => g.Topics).Returns(topics);
            var controller = new TopicController(_topicsDataContext.Object);
            SetUpHttpRequestParameters(controller);
            var response = controller.Post(new Topic
            {
                Name = "Visual Studio on a Mac",
                Tutorials = new List<Tutorial> { }
            });
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            ExpandoObject expando;
            Assert.IsTrue(response.TryGetContentValue(out expando));
            var expandoDict = (IDictionary<string, object>)expando;
            Assert.AreEqual(0, expandoDict["id"]);
            Assert.AreEqual("http://localhost/api/Topic/0", expandoDict["url"]);
            _topicsDataContext.Verify(c => c.SaveChanges(), Times.Once());

        }

        private void SetUpHttpRequestParameters(TopicController controller)
        {
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Topic");
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = new HttpConfiguration();
        }
    }
}
