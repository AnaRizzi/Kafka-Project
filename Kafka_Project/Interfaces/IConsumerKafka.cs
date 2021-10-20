using Kafka_Project.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.Interfaces
{
    public interface IConsumerKafka
    {
        void GetMessage(Action<KafkaMessageConsumer> action);
    }
}
