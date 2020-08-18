using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace RallyDakar.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configura��es NLog (NLog.Web.AspNetCore)
            var logger = NLogBuilder
                .ConfigureNLog("nlog.config")
                .GetCurrentClassLogger();

            logger.Info("Iniciando o aplicativo...");

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex) {
                logger.Error(ex, "Aplica��o parou de rodar");
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>()
                        .UseNLog();
                });
    }
}