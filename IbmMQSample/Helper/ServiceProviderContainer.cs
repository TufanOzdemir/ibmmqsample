using System;

namespace IbmMQSample.Helper
{
    public class ServiceProviderContainer
    {
        public readonly static ServiceProviderContainer Instance = new ServiceProviderContainer();
        private IServiceProvider _serviceProvider;
        private ServiceProviderContainer()
        {
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetService(Type type)
        {
            return _serviceProvider.GetService(type);
        }
    }
}