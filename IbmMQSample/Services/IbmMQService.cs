using IBM.XMS;
using IbmMQSample.Helper;
using IbmMQSample.Interface;
using IbmMQSample.Models;
using System;

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

            producer.Close();
            connectionWMQ.Close();
            destination.Dispose();
            connectionWMQ.Dispose();
            Console.WriteLine("Send Successfuly");
        }

        public void Publish(string value)
        {
            var connectionFactory = (IConnectionFactory)ServiceProviderContainer.Instance.GetService(typeof(IConnectionFactory));
            var mqConfigModel = (IbmMQConfigModel)ServiceProviderContainer.Instance.GetService(typeof(IbmMQConfigModel));

            var connectionWMQ = connectionFactory.CreateConnection();
            var sessionWMQ = connectionWMQ.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            var destination = sessionWMQ.CreateTopic(mqConfigModel.TopicName);
            destination.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, XMSC.WMQ_TARGET_DEST_MQ);
            var producer = sessionWMQ.CreateProducer(destination);
            connectionWMQ.Start();

            var textMessage = sessionWMQ.CreateTextMessage();
            textMessage.Text = value;
            producer.Send(textMessage);

            producer.Close();
            connectionWMQ.Close();
            destination.Dispose();
            connectionWMQ.Dispose();
            Console.WriteLine("Publish Successfuly");
        }

        public void Listen(int second = 1)
        {
            var connectionFactory = (IConnectionFactory)ServiceProviderContainer.Instance.GetService(typeof(IConnectionFactory));
            var mqConfigModel = (IbmMQConfigModel)ServiceProviderContainer.Instance.GetService(typeof(IbmMQConfigModel));

            var connection = connectionFactory.CreateConnection();
            connection.ExceptionListener = new ExceptionListener(IbmMQService.OnException);

            var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            IDestination topic = session.CreateQueue(mqConfigModel.QueueName);
            IMessageConsumer consumer = session.CreateConsumer(topic);
            connection.Start();

            Console.WriteLine("Waiting for messages....");
            for (int i = 0; i < second; i++)
            {
                var textMessage = (ITextMessage)consumer.Receive(1000);
                if (textMessage != null)
                {
                    Console.WriteLine(textMessage.Text);
                    if (textMessage.Text.Equals("Exit"))
                    {
                        break;
                    }
                }
            }
            connection.Close();
            consumer.Close();
            topic.Dispose();
        }

        public void Subscribe(string topicName)
        {
            var connectionFactory = (IConnectionFactory)ServiceProviderContainer.Instance.GetService(typeof(IConnectionFactory));
            var mqConfigModel = (IbmMQConfigModel)ServiceProviderContainer.Instance.GetService(typeof(IbmMQConfigModel));

            var connection = connectionFactory.CreateConnection();
            connection.ExceptionListener = new ExceptionListener(IbmMQService.OnException);

            var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            IDestination topic = session.CreateTopic(topicName);
            IMessageConsumer subscriber = session.CreateConsumer(topic);
            connection.Start();

            Console.WriteLine("Waiting for messages....");
            while(true)
            {
                var textMessage = (ITextMessage)subscriber.Receive(1000);
                if (textMessage != null)
                {
                    Console.WriteLine(textMessage.Text);
                    if (textMessage.Text.Equals("Exit"))
                    {
                        break;
                    }
                }
            }
            connection.Close();
            subscriber.Close();
            topic.Dispose();
        }

        static void OnException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}