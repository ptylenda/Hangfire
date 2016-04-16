using System;
using Hangfire;
using Hangfire.Filters;

namespace ConsoleSampleContext
{
    class ExampleJob : IJob<string>
    {
        public string Run()
        {
            Console.WriteLine("Test");
            Console.WriteLine("I am " + JobContext.JobId);
            
            JobContext.UpdateStatus("dupeczka");


            JobContext.UpdateStatus("hehe");

            return string.Format("Returned data from {0}!!!!", JobContext.JobId);
        }
    }
}
