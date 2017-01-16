using System;
using System.Globalization;
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
