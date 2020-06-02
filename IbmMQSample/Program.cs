using CommandDotNet;
using IBM.XMS;
using IbmMQSample.Business;
using IbmMQSample.Helper;
using IbmMQSample.Interface;
using IbmMQSample.Models;
using IbmMQSample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IbmMQSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IServiceCollection services = new ServiceCollection();
                var configuration = new ConfigurationBuilder()
                                      .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //Buranın çalışması için csproj'a ekleme yapıldı. Additions to csproj for this place to work.
                                      .AddJsonFile("appconfig.json")
                                      .Build();
                Startup(services, configuration);
                AppRunner<MainView> consoleRunner = new AppRunner<MainView>();
                consoleRunner.Run(args);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void Startup(IServiceCollection services, IConfigurationRoot configuration)
        {
            RegisterXMS(services, configuration);
            services.AddSingleton<IQueueManager, IbmMQService>();
            ServiceProviderContainer.Instance.Initialize(services.BuildServiceProvider());
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