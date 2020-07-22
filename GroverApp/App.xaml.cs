using GroverApp.Repos;
using GroverApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace GroverApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            ConfigureLogger();
            ServiceCollection services = new ServiceCollection();
            services.AddDbContext<EmployeesDatabaseContext>(options =>
            {
                options.UseSqlite("Data Source = GroverDB.db");
            });

            services.AddSingleton<MainWindow>();
            services.AddSingleton<EmployeeDataService>();
            serviceProvider = services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureLogger()
        {
            var logConfig = new LoggingConfiguration();

            var fileTarget = new FileTarget
            {
                Name = "fileLog",
                FileName = "${basedir}/Logs/info.log",
                CreateDirs = true,
                Layout = "${longdate} Thr: ${threadid} - [${level}] - ${message} ${exception}",
                ArchiveFileName = $"./Logs/Archives/{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.log",
                ArchiveOldFileOnStartup = true
            };

            logConfig.AddTarget(fileTarget);
            logConfig.AddRuleForAllLevels(fileTarget);
            LogManager.Configuration = logConfig;
        }
    }
}
