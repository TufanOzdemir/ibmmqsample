using IBM.XMS;
using IbmMQSample.Interface;
using IbmMQSample.Models;
using System;

namespace IbmMQSample.Services
{
    public class IbmMQService : IQueueManager
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly IbmMQConfigModel _mqConfigModel;

        public IbmMQService(IConnectionFactory connectionFactory, IbmMQConfigModel mqConfigModel)
        {
            _connectionFactory = connectionFactory;
            _mqConfigModel = mqConfigModel;
        }

        public void Send(string value)
        {
            SendAndPublish(false, value);
        }

        public void Publish(string value)
        {
            SendAndPublish(true, value);
        }

        public void Listen(int second = 1)
        {
            var connection = _connectionFactory.CreateConnection();
            connection.ExceptionListener = IbmMQService.OnException;

            var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            IDestination topic = session.CreateQueue(_mqConfigModel.QueueName);
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
            var connection = _connectionFactory.CreateConnection();
            connection.ExceptionListener = IbmMQService.OnException;

            var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            IDestination topic = session.CreateTopic(topicName);
            IMessageConsumer subscriber = session.CreateConsumer(topic);
            connection.Start();

            Console.WriteLine("Waiting for messages....");
            while (true)
            {
                var textMessage = (ITextMessage)subscriber.Receive();
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

        private void SendAndPublish(bool isPublish, string value)
        {
            var connectionWMQ = _connectionFactory.CreateConnection();
            var sessionWMQ = connectionWMQ.CreateSession(false, AcknowledgeMode.AutoAcknowledge);
            var destination = GetDestination(isPublish, sessionWMQ);
            var producer = sessionWMQ.CreateProducer(destination);
            connectionWMQ.Start();

            var textMessage = sessionWMQ.CreateTextMessage();
            textMessage.Text = value;
            producer.Send(textMessage);

            producer.Close();
            connectionWMQ.Close();
            destination.Dispose();
            connectionWMQ.Dispose();
            Console.WriteLine("Successfuly");
        }

        private IDestination GetDestination(bool isPublish, ISession session)
        {
            IDestination result = null;
            if (isPublish)
            {
                result = session.CreateTopic(_mqConfigModel.TopicName);
                result.SetIntProperty(XMSC.WMQ_TARGET_CLIENT, XMSC.WMQ_TARGET_DEST_MQ);
            }
            else
            {
                result = session.CreateQueue(_mqConfigModel.QueueName);
            }
            return result;
        }

        static void OnException(Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}