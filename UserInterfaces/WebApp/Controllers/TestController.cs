namespace WebApp.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Http;
    using Application.Tests;
    using Application.Tests.Models;
    using Newtonsoft.Json;

    [AllowAnonymous]
    public class TestController : ApiController
    {
        private readonly TestAppService appService;

        public TestController(TestAppService appService)
        {
            this.appService = appService;
        }

        [HttpGet]
        public IHttpActionResult Get(Guid id)
        {
            return Ok(appService.Get(id));
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            return Ok(appService.GetAll());
        }

        [HttpPost]
        public IHttpActionResult CreateOrUpdate(TestBindModel model)
        {
            return Ok(appService.CreateOrUpdate(model));
        }

        [HttpPost]
        public IHttpActionResult TestTransaction()
        {
            appService.TestTransaction();

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult TestPost()
        {
            var request = HttpContext.Current.Request;

            var model = JsonConvert.DeserializeObject<Model>(request.Params["PostData"]);

            var files = request.Files.AllKeys.Select(m =>
            {
                var ms = new MemoryStream();

                var file = request.Files.Get(m);

                file.InputStream.CopyTo(ms);

                return new { file.FileName, Stream = ms };
            }).ToDictionary(k => k.FileName, v => v.Stream);

            return Ok(new { model, files = files.Select(m => new { fileName = m.Key, fileLength = m.Value.Length }) });
        }

        public class Model
        {
            public string Name { get; set; }

            public string IdNo { get; set; }
        }
    }
}
