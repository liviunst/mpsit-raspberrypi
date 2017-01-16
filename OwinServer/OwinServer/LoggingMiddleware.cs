using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace OwinServer
{
    public class LoggingMiddleware : OwinMiddleware
    {
        readonly Random _rand;

        public LoggingMiddleware(OwinMiddleware next) : base(next)
        {
            _rand = new Random();
        }

        public override async Task Invoke(IOwinContext context)
        {
            var id = _rand.Next();

            Console.WriteLine($"{id} Incoming request " + DateTime.UtcNow + " From: " + context.Request.RemoteIpAddress);

            await this.Next.Invoke(context);

            Console.WriteLine($"{id} Finished request " + DateTime.UtcNow);

        }
    }
}
