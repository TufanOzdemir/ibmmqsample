namespace IbmMQSample.Interface
{
    public interface IQueueManager
    {
        /// <summary>
        /// Send your messages
        /// Mesaj gönderir.
        /// </summary>
        /// <param name="value"></param>
        void Send(string value);

        /// <summary>
        /// Publish your messages
        /// Mesaj yayınlar.
        /// </summary>
        /// <param name="value"></param>
        void Publish(string value);

        /// <summary>
        /// It's listen queue
        /// Kuyruğu dinler.
        /// </summary>
        /// <param name="second"></param>
        void Listen(int second);

        /// <summary>
        /// It's listen publisher
        /// Yayıncıyı dinler.
        /// </summary>
        /// <param name="second"></param>
        void Subscribe(string topicName);
    }
}