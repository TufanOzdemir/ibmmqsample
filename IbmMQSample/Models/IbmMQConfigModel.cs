namespace IbmMQSample.Models
{
    public class IbmMQConfigModel
    {
        public const string ConfigSection = "IbmMQ"; 
        public string Host { get; set; }
        public int Port { get; set; }
        public string Channel { get; set; }
        public string User { get; set; }
        public string ManagerName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}