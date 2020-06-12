using IbmMQSample.Interface;
using CommandDotNet;

namespace IbmMQSample.Business
{
    public class MainView
    {
        private readonly IQueueManager _queueManager;

        public MainView(IQueueManager queueManager)
        {
            _queueManager = queueManager;
        }

        [Command(Description = "Send Message to queue")]
        public void Send(string message)
        {
            _queueManager.Send(message);
        }

        [Command(Description = "Listen Messages from queue")]
        public void Listen([Option(LongName = "Second", ShortName = "s", Description = "Time for listen.")]int second = 1)
        {
            _queueManager.Listen(second);
        }

        [Command(Description = "Publish Message to subscriber")]
        public void Publish(string message)
        {
            _queueManager.Publish(message);
        }

        [Command(Description = "Listen Messages from publisher")]
        public void Subscribe([Option(LongName = "topic", ShortName = "n", Description = "Topic name for subscribe")]string topicName)
        {
            _queueManager.Subscribe(topicName);
        }
    }
}