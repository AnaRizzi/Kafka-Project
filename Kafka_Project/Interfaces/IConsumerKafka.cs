using Kafka_Project.Models;
using System;
using System.Threading.Tasks;

namespace Kafka_Project.Interfaces
{
    public interface IConsumerKafka
    {
        void GetMessage(Func<KafkaMessageConsumer, Task> action);
        Task SendDeadLetter(KafkaMessageConsumer message);
    }
}
