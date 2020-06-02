using CommandDotNet.Attributes;
using IbmMQSample.Helper;
using IbmMQSample.Interface;
using System;

namespace IbmMQSample.Business
{
    public class MainView
    {
        [ApplicationMetadata(Description = "Send Message to queue")]
        public void Send(string message)
        {
            try
            {
                var mqservice = (IQueueManager)ServiceProviderContainer.Instance.GetService(typeof(IQueueManager));
                mqservice.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [ApplicationMetadata(Description = "Listen Messages from queue")]
        public void Listen([Option(LongName = "Second", ShortName = "s", Description = "Time for listen.")]int second = 1)
        {
            try
            {
                var mqservice = (IQueueManager)ServiceProviderContainer.Instance.GetService(typeof(IQueueManager));
                mqservice.Listen(second);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}