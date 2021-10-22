using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.Models
{
    public class KafkaMessageProducer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RandomNumber { get; set; }

        public KafkaMessageProducer(KafkaMessageConsumer message)
        {
            Id = message.Id;
            Name = message.Name;
            RandomNumber = new Random().Next(1, 99);
        }
    }
}
