using System;
using System.Threading;
using IbmMQSample.Interface;

namespace IbmMQSample.Services
{
    public class MockMQService : IQueueManager
    {
        public void Send(string value)
        {
            if (value == "throw")
            {
                throw new Exception("Send error");
            }
            Console.WriteLine($"{nameof(Send)}: {value}");
        }

        public void Publish(string value)
        {
            if (value == "throw")
            {
                throw new Exception("Publish error");
            }
            Console.WriteLine($"{nameof(Publish)}: {value}");
        }

        public void Listen(int second)
        {
            if (second == 999)
            {
                throw new Exception("Listen error");
            }
            for (int i = second; i > 0; i--)
            {
                Console.WriteLine($"{nameof(Listen)} for {i} seconds");
                Thread.Sleep(1000);
            }
        }

        public void Subscribe(string topicName)
        {
            if (topicName == "throw")
            {
                throw new Exception("Subscribe error");
            }
            Console.WriteLine($"{nameof(Subscribe)}: {topicName}");
        }
    }
}