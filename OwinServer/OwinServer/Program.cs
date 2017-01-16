using System;
using System.Web.Http;
using Microsoft.Owin.Hosting;
using Owin;

namespace OwinServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://*:9000/";
            Console.ForegroundColor = ConsoleColor.Green;

            var context = new MpsitModel();
            context.Commands.Add(new Command() {Value = "test"});
            context.SaveChanges();

            using (WebApp.Start(baseAddress, appbuilder =>
            {
                HttpConfiguration config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();

                appbuilder.Use<LoggingMiddleware>();
                appbuilder.UseWebApi(config);
            }))
            {
                Console.WriteLine("Starting server");
                Console.WriteLine("Press any key to shutdown");
                Console.WriteLine();
                Console.ReadLine();
            }
        }
    }
}
