using CommandDotNet.Attributes;
using IBM.XMS;
using IbmMQSample.Helper;
using IbmMQSample.Interface;
using IbmMQSample.Models;
using System;
using System.Threading;

namespace IbmMQSample.Services
{
    public class IbmMQService : IQueueManager
    {
        public void Send(string value)
        {
            var connectionFactory = (IConnectionFactory)ServiceProviderContainer.Instance.GetService(typeof(IConnectionFactory));
            var mqConfigModel = (IbmMQConfigModel)ServiceProviderContainer.Instance.GetService(typeof(IbmMQConfigModel));

            var connectionWMQ = connectionFactory.CreateConnection();
            var sessionWMQ = connectionWMQ.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            var destination = sessionWMQ.CreateQueue(mqConfigModel.QueueName);
            var producer = sessionWMQ.CreateProducer(destination);
            connectionWMQ.Start();
            var textMessage = sessionWMQ.CreateTextMessage();
            textMessage.Text = value;
            producer.Send(textMessage);
            connectionWMQ.Stop();
            Console.WriteLine("Send Successfuly");
        }

        public void Listen([Option(LongName = "Second", ShortName = "s", Description = "Time for listen.")]int second = 1)
        {
            var connectionFactory = (IConnectionFactory)ServiceProviderContainer.Instance.GetService(typeof(IConnectionFactory));
            var mqConfigModel = (IbmMQConfigModel)ServiceProviderContainer.Instance.GetService(typeof(IbmMQConfigModel));

            var connection = connectionFactory.CreateConnection();
            connection.ExceptionListener = new ExceptionListener(IbmMQService.OnException);

            var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            IDestination topic = session.CreateQueue(mqConfigModel.QueueName);
            IMessageConsumer consumer = session.CreateConsumer(topic);
            consumer.MessageListener = new MessageListener(IbmMQService.OnMessage);
            connection.Start();

            for (int i = 0; i < second; i++)
            {
                Console.WriteLine("Waiting for messages....");
                Thread.Sleep(1000);
            }
            connection.Stop();
        }

        static void OnMessage(IMessage msg)
        {
            Console.WriteLine(msg);
        }

        static void OnException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}