using mocking_ef_datacontext.DB;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace mocking_ef_datacontext.Controllers
{
    public class TopicController : ApiController
    {
        ITopicsDataContext _db;
        public TopicController() : this(new TopicsDataContext())
        {
        }
        public TopicController(ITopicsDataContext topicDataContext)
        {
            _db = topicDataContext;
        }

        // GET api/topics
        [Route("api/topics")]
        //public HttpResponseMessage Get()
        //{
        //    using (var db = new TopicsDataContext())
        //    {
        //        var topics = db.Topics
        //                       .Select(t => new
        //                       {
        //                           id = t.Id,
        //                           name = t.Name
        //                       })
        //                      .ToList();
        //        return Request.CreateResponse(HttpStatusCode.OK, topics);
        //    }
        //}
        public HttpResponseMessage Get()
        {
            var topics = _db.Topics
                           .ToList()
                           .Select(t =>
                           {
                               dynamic expando = new ExpandoObject();
                               expando.id = t.Id;
                               expando.name = t.Name;
                               return expando;
                           })
                          .ToList();
            return Request.CreateResponse(HttpStatusCode.OK, topics);
        }

        // GET api/topic/2
        //public HttpResponseMessage Get(int id)
        //{
        //    using (var db = new TopicsDataContext())
        //    {
        //        var topic = db.Topics.SingleOrDefault(t => t.Id == id);
        //        if (topic == null)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, new
        //            {
        //                message = "The topic you requested was not found"
        //            });
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            id = topic.Id,
        //            name = topic.Name,
        //            tutorials = topic.Tutorials
        //                             .Select(t => new
        //                             {
        //                                 id = t.Id,
        //                                 name = t.Name,
        //                                 website = t.WebSite,
        //                                 type = t.Type,
        //                                 url = t.Url
        //                             })
        //        });
        //    }
        //}
        public HttpResponseMessage Get(int id)
        {
            var topic = _db.Topics.SingleOrDefault(t => t.Id == id);
            if (topic == null)
            {
                dynamic expando = new ExpandoObject();
                expando.message = "The topic you requested was not found";
                return Request.CreateResponse(HttpStatusCode.NotFound, expando as object);
            }
            dynamic obj = new ExpandoObject();
            obj.id = topic.Id;
            obj.name = topic.Name;
            obj.tutorials = topic.Tutorials
                                 .Select(t =>
                                 {
                                     dynamic expando = new ExpandoObject();
                                     expando.id = t.Id;
                                     expando.name = t.Name;
                                     expando.website = t.WebSite;
                                     expando.type = t.Type;
                                     expando.url = t.Url;
                                     return expando;
                                 });
            return Request.CreateResponse(HttpStatusCode.OK, obj as object);
        }

        [Route("api/topic/{id}/{name}")]
        // GET api/topic/2
        //public HttpResponseMessage Get(int id, string name)
        //{
        //    using (var db = new TopicsDataContext())
        //    {
        //        var tutorials = db.Topics.Where(t => t.Id == id)
        //                             .SelectMany(t => t.Tutorials)
        //                             .Where(t => t.Name == name)
        //                             .ToList();

        //        if (tutorials.Count == 0)
        //        {
        //            return Request.CreateResponse(HttpStatusCode.NotFound, new
        //            {
        //                message = "The tutorial  you requested was not found"
        //            });
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            id = id,
        //            name = name,
        //            tutorials = tutorials
        //                         .Select(t => new
        //                         {
        //                             id = t.Id,
        //                             name = t.Name,
        //                             website = t.WebSite,
        //                             type = t.Type,
        //                             url = t.Url
        //                         })
        //        });
        //    }
        //}
        public HttpResponseMessage Get(int id, string name)
        {
            var tutorials = _db.Topics.Where(t => t.Id == id)
                                 .SelectMany(t => t.Tutorials)
                                 .Where(t => t.Name == name)
                                 .ToList();

            if (tutorials.Count == 0)
            {
                dynamic expando = new ExpandoObject();
                expando.message = "The tutorial  you requested was not found";
                return Request.CreateResponse(HttpStatusCode.NotFound, expando as object);
            }

            dynamic obj = new ExpandoObject();
            obj.id = id;
            obj.name = name;
            obj.tutorials = tutorials
                             .Select(t =>
                             {
                                 dynamic expando = new ExpandoObject();
                                 expando.id = t.Id;
                                 expando.name = t.Name;
                                 expando.website = t.WebSite;
                                 expando.type = t.Type;
                                 expando.url = t.Url;
                                 return expando;
                             })
                             .ToList();
            return Request.CreateResponse(HttpStatusCode.OK, obj as object);
        }
        // POST api/values
        //public HttpResponseMessage Post(Topic topic)
        //{
        //    using (var db = new TopicsDataContext())
        //    {
        //        db.Topics.Add(topic);
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            id = topic.Id,
        //            url = Request.RequestUri.AbsoluteUri + "/" + topic.Id
        //        });
        //    }
        //}
        public HttpResponseMessage Post(Topic topic)
        {
            _db.Topics.Add(topic);
            _db.SaveChanges();
            dynamic expando = new ExpandoObject();
            expando.id = topic.Id;
            expando.url = Request.RequestUri.AbsoluteUri + "/" + topic.Id;
            return Request.CreateResponse(HttpStatusCode.OK, expando as object);
        }

        //public HttpResponseMessage Put(Topic topic)
        //{
        //    using (var db = new TopicsDataContext())
        //    {
        //        db.Topics.Attach(topic);
        //        db.Entry(topic).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return Request.CreateResponse(HttpStatusCode.OK, new
        //        {
        //            message = "topic is updated successfully"
        //        });
        //    }
        //}
        public HttpResponseMessage Put(Topic topic)
        {
            _db.Topics.Attach(topic);
            _db.Entry(topic).State = EntityState.Modified;
            _db.SaveChanges();
            dynamic expando = new ExpandoObject();
            expando.message = "topic is updated successfully";
            return Request.CreateResponse(HttpStatusCode.OK, expando as object);
        }
    }
}
