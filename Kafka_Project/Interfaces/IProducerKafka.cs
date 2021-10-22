using Kafka_Project.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kafka_Project.Interfaces
{
    public interface IProducerKafka
    {
        Task Publish(Message<KafkaMessageProducer> message);
    }
}
