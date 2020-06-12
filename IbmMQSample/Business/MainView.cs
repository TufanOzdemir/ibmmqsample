using IbmMQSample.Helper;
using IbmMQSample.Interface;
using System;
using CommandDotNet;

namespace IbmMQSample.Business
{
    public class MainView
    {
        [Command(Description = "Send Message to queue")]
        public void Send(string message)
        {
            var mqservice = (IQueueManager)ServiceProviderContainer.Instance.GetService(typeof(IQueueManager));
            mqservice.Send(message);
        }

        [Command(Description = "Listen Messages from queue")]
        public void Listen([Option(LongName = "Second", ShortName = "s", Description = "Time for listen.")]int second = 1)
        {
            var mqservice = (IQueueManager)ServiceProviderContainer.Instance.GetService(typeof(IQueueManager));
            mqservice.Listen(second);
        }

        [Command(Description = "Publish Message to subscriber")]
        public void Publish(string message)
        {
            var mqservice = (IQueueManager)ServiceProviderContainer.Instance.GetService(typeof(IQueueManager));
            mqservice.Publish(message);
        }

        [Command(Description = "Listen Messages from publisher")]
        public void Subscribe([Option(LongName = "topic", ShortName = "n", Description = "Topic name for subscribe")]string topicName)
        {
            var mqservice = (IQueueManager)ServiceProviderContainer.Instance.GetService(typeof(IQueueManager));
            mqservice.Subscribe(topicName);
        }
    }
}