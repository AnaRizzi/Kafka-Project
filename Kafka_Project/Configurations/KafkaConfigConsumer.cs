using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.Configurations
{
    public class KafkaConfigConsumer
    {
        public string BootstrapServers { get; set; }
        public string GroupId { get; set; }
        public string Topic { get; set; }
        public string DeadLetter { get; set; }

    }
}
