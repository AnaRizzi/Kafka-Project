using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.Models
{
    public class Message<T>
    {
        public Guid CorrelationId { get; set; }
        public T Payload { get; set; }

        public Message(T payload)
        {
            CorrelationId = Guid.NewGuid();
            Payload = payload;
        }
    }
}
