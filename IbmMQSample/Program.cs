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
                var services = new ServiceCollection();
                var configModel = LoadConfiguration(services);
                var consoleRunner = new AppRunner<MainView>()
                    .UseMicrosoftDependencyInjection(ConfigureServiceProvider(services, configModel))
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

        private static IbmMQConfigModel LoadConfiguration(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //Buranın çalışması için csproj'a ekleme yapıldı. Additions to csproj for this place to work.
                .AddJsonFile("appconfig.json")
                .Build();

            var configModel = new IbmMQConfigModel();
            var section = configuration.GetSection(IbmMQConfigModel.ConfigSection);
            section.Bind(configModel);
            services.AddSingleton(configModel);
            return configModel;
        }

        static ServiceProvider ConfigureServiceProvider(ServiceCollection services, IbmMQConfigModel configModel)
        {
            services.AddTransient<MainView>();
            if (configModel.UseMock)
            {
                services.AddSingleton<IQueueManager, MockMQService>();
            }
            else
            {
                RegisterXMS(services, configModel);
                services.AddSingleton<IQueueManager, IbmMQService>();
            }
            return services.BuildServiceProvider();
        }

        static void RegisterXMS(IServiceCollection services, IbmMQConfigModel configModel)
        {
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