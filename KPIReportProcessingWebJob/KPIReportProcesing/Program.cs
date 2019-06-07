using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs;sssssssssssssssssss
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;


namespace KPIReportProcesing
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();



            var host2 = new JobHost();
            SupportService _supportService = new SupportService();
            _supportService.Initialize();
            _supportService.SetPoolProvisioningConfigurations();
            host.CallAsync(typeof(SupportService).GetMethod("SetPoolProvisioningConfigurations")



        }
    }
}



public class Program
{
    static JobHost host = null;
    static void Main(string[] args)
    {
        var cancellationToken = new WebJobsShutdownWatcher().Token;
        cancellationToken.Register(() =>
        {
            // gracefully stops activities 
            // delete process dependencies
            // flush telemetry
            // etc

            host.Stop(); // allow to go through the RunAndBlock()
        });

        // HostId is mandatory
        // Both ConnectionString can be set to null preventing any dependencies
        host = new JobHost(new JobHostConfiguration { HostId = Guid.NewGuid().ToString().Substring(0, 32), StorageConnectionString = string.Empty, DashboardConnectionString = string.Empty });
        host.Call(typeof(Program).GetMethod("DoLongStuff"));
        host.RunAndBlock();

        Trace.TraceInformation("webjob fully terminated");
    }

    [NoAutomaticTrigger]
    public void DoLongStuff()
    {
        // do stuff
    }
}


static void Main()
{
    var host = new JobHost();
    // The following code ensures that the WebJob will be running continuously
    host.RunAndBlock();
}

static void Main()
{
    var config = new JobHostConfiguration { DashboardConnectionString = null };
    var host = new JobHost(config);
    host.RunAndBlock();
}