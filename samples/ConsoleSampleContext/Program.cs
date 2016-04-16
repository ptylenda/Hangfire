using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;

namespace ConsoleSampleContext
{
    class Program
    {
        static void Main(string[] args)
        {

            GlobalConfiguration.Configuration
                .UseColouredConsoleLogProvider()
                .UseSqlServerStorage(@"Server=.;Database=Hangfire;Trusted_Connection=True;");

            BackgroundJob.Enqueue(new ExampleJob());
        }
    }
}
