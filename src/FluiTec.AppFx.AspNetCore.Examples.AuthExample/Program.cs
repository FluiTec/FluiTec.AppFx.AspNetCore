using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace FluiTec.AppFx.AspNetCore.Examples.AuthExample
{
    /// <summary>A program.</summary>
    public class Program
    {
        /// <summary>Main entry-point for this application.</summary>
        /// <param name="args"> An array of command-line argument strings. </param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>Builds web host.</summary>
        /// <param name="args"> An array of command-line argument strings. </param>
        /// <returns>An IWebHost.</returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}