using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;

namespace OwinServer
{
    [RoutePrefix("api/rasp")]
    public class RaspberryController : ApiController
    {
        private static MpsitModel context = new MpsitModel(); 

        [HttpGet]
        [Route("command")]
        public string GetCommand()
        {
            return "Nothing";
        }

        [HttpPost]
        [Route("picture")]
        public string PostPicture()
        {
            var provider = new MultipartMemoryStreamProvider();
            Request.Content.ReadAsMultipartAsync(provider).Wait();
            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var buffer = file.ReadAsByteArrayAsync().Result;

                var im = new Image()
                {
                    Name = filename,
                    Content = buffer
                };

                context.Images.Add(im);
                context.SaveChanges();
            }
            return "ok";
        }

        [HttpPut]
        [Route("temperature")]
        public string PutTemperature()
        {
            var result = Request.Content.ReadAsStringAsync().Result;
            double doubleResult;
            double.TryParse(result, out doubleResult);

            var entry = new Temperature()
            {
                Date = DateTime.Now,
                Value = doubleResult
            };

            context.Temperatures.Add(entry);
            context.SaveChanges();

            return "Ok";
        }
    }
}
