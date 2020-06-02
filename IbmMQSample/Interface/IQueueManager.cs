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
        /// It's listen queue
        /// Kuyruğu dinler.
        /// </summary>
        /// <param name="second"></param>
        void Listen(int second);
    }
}