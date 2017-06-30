using mocking_ef_datacontext.DB;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mocking_ef_datacontext.Controllers
{
    public class TopicController : ApiController
    {
        // GET api/topics
        [Route("api/topics")]
        public HttpResponseMessage Get()
        {
            using (var db = new TopicsDataContext())
            {
                var topics = db.Topics
                               .Select(t => new
                               {
                                   id = t.Id,
                                   name = t.Name
                               })
                              .ToList();
                return Request.CreateResponse(HttpStatusCode.OK, topics);
            }
        }

        // GET api/topic/2
        public HttpResponseMessage Get(int id)
        {
            using (var db = new TopicsDataContext())
            {
                var topic = db.Topics.SingleOrDefault(t => t.Id == id);
                if (topic == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new
                    {
                        message = "The topic you requested was not found"
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    id = topic.Id,
                    name = topic.Name,
                    tutorials = topic.Tutorials
                                     .Select(t => new
                                     {
                                         id = t.Id,
                                         name = t.Name,
                                         website = t.WebSite,
                                         type = t.Type,
                                         url = t.Url
                                     })
                });
            }
        }

        [Route("api/topic/{id}/{name}")]
        // GET api/topic/2
        public HttpResponseMessage Get(int id, string name)
        {
            using (var db = new TopicsDataContext())
            {
                var tutorials = db.Topics.Where(t => t.Id == id)
                                     .SelectMany(t => t.Tutorials)
                                     .Where(t => t.Name == name)
                                     .ToList();

                if (tutorials.Count == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, new
                    {
                        message = "The tutorial  you requested was not found"
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    id = id,
                    name = name,
                    tutorials = tutorials
                                 .Select(t => new
                                 {
                                     id = t.Id,
                                     name = t.Name,
                                     website = t.WebSite,
                                     type = t.Type,
                                     url = t.Url
                                 })
                });
            }
        }

        // POST api/values
        public HttpResponseMessage Post(Topic topic)
        {
            using (var db = new TopicsDataContext())
            {
                db.Topics.Add(topic);
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    id = topic.Id,
                    url = Request.RequestUri.AbsoluteUri + "/" + topic.Id
                });
            }
        }
         
        public HttpResponseMessage Put(Topic topic)
        {
            using (var db = new TopicsDataContext())
            {
                db.Topics.Attach(topic);
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    message = "topic is updated successfully"
                });
            }
        }
    }
}
