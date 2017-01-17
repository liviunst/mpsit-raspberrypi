using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Web.Http;
using Newtonsoft.Json;

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
            try
            {
                var commands = context.Commands.Where(x => x.Sent == false).OrderBy(x => x.Id);
                var firstCommand = commands.First();
                firstCommand.Sent = true;
                context.SaveChanges();
                return firstCommand.Value;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }

            return "No Command";
        }

        [HttpPut]
        [Route("command")]
        public string PutCommand()
        {
            var result = Request.Content.ReadAsStringAsync().Result;

            context.Commands.Add(new Command()
            {
                Sent = false,
                Value = result
            });

            context.SaveChanges();

            return "ok";
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

        [HttpGet]
        [Route("temperature")]
        public string GetTemperature()
        {
            var temperatures = context.Temperatures.ToList();
            var count = temperatures.Count;
            var record = temperatures.Skip(Math.Max(0, count - 1)).FirstOrDefault();

            var result = "x" + record.Id + "x" + record.Value + "x";

            return result;
        }
    }
}
