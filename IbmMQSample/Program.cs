using CommandDotNet;
using IBM.XMS;
using IbmMQSample.Business;
using IbmMQSample.Interface;
using IbmMQSample.Models;
using IbmMQSample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using CommandDotNet.IoC.MicrosoftDependencyInjection;

namespace IbmMQSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                                      .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //Buranın çalışması için csproj'a ekleme yapıldı. Additions to csproj for this place to work.
                                      .AddJsonFile("appconfig.json")
                                      .Build();
                var consoleRunner = new AppRunner<MainView>()
                    .UseMicrosoftDependencyInjection(ConfigureServiceProvider(configuration))
                    .UseErrorHandler((ctx, ex) =>
                    {
                        ctx.Console.WriteLine(ex.Message);
                        return ExitCodes.Error.Result;
                    });
                consoleRunner.Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static ServiceProvider ConfigureServiceProvider(IConfigurationRoot configuration)
        {
            var services = new ServiceCollection();
            services.AddTransient<MainView>();
            RegisterXMS(services, configuration);
            services.AddSingleton<IQueueManager, IbmMQService>();
            return services.BuildServiceProvider();
        }

        static void RegisterXMS(IServiceCollection services, IConfigurationRoot configuration)
        {
            //Register ConfigModel
            var configModel = new IbmMQConfigModel();
            var section = configuration.GetSection(IbmMQConfigModel.ConfigSection);
            section.Bind(configModel);
            services.AddSingleton(configModel);

            //Register ConnectionFactory
            var factoryFactory = XMSFactoryFactory.GetInstance(XMSC.CT_WMQ);
            var connectionFactory = factoryFactory.CreateConnectionFactory();
            connectionFactory.SetStringProperty(XMSC.WMQ_HOST_NAME, configModel.Host);
            connectionFactory.SetIntProperty(XMSC.WMQ_PORT, configModel.Port);
            connectionFactory.SetStringProperty(XMSC.WMQ_CHANNEL, configModel.Channel);
            connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, configModel.ManagerName);
            connectionFactory.SetStringProperty(XMSC.USERID, configModel.User);
            connectionFactory.SetStringProperty(XMSC.PASSWORD, configModel.Password);
            services.AddSingleton(connectionFactory);
        }
    }
}