using System.Web.Http;

namespace OwinServer
{
    [RoutePrefix("api/rasp")]
    public class RaspberryController : ApiController
    {
        [HttpGet]
        [Route("command")]
        public string GetCommand()
        {
            return "Nothing";
        }

        [HttpPut]
        [Route("temperature")]
        public string PutTemperatur()
        {
            return "Ok";
        }
    }
}
