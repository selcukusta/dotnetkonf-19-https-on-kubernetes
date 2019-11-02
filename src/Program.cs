using System;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(options =>
                {
                    if (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("CONTAINS_CERTIFICATE")) && Environment.GetEnvironmentVariable("CONTAINS_CERTIFICATE").Equals("1"))
                    {
                        options.Listen(IPAddress.Any, 443, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                            listenOptions.UseHttps
                            (
                                Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path"), Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password")
                            );
                        });
                    }

                })
                .UseStartup<Startup>();
    }
}
