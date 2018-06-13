using System;
using System.Collections.Generic;
using System.Configuration;
using CS.Common.External.Interfaces;
using SFA.DAS.Payments.DCFS.Context;
using SFA.DAS.Payments.Reference.Commitments;

namespace ManualTaskRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new AppSettingsReader();

            var baseUrl = settings.GetValue("EventsApiUrl", typeof(string)) as string;
            var token = settings.GetValue("EventsApiToken", typeof(string)) as string;
            var dbConnectionString = settings.GetValue("ConnectionString", typeof(string)) as string;
            
            try
            {
                var context = new TextContext(dbConnectionString, baseUrl, token);
                var task = new ImportCommitmentsTask();

                task.Execute(context);

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.ToString());
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }

    internal class TextContext : IExternalContext
    {
        public TextContext(string transientConnectionString, string baseUrl, string token)
        {
            Properties = new Dictionary<string, string>
            {
                { ContextPropertyKeys.TransientDatabaseConnectionString, transientConnectionString},
                { ContextPropertyKeys.LogLevel, "DEBUG"},
                { "DAS.Payments.Commitments.BaseUrl", baseUrl},
                { "DAS.Payments.Commitments.ClientToken", token}
            };
        }

        public IDictionary<string, string> Properties { get; set; }
    }
}
