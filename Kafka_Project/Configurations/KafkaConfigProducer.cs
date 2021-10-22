using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.Configurations
{
    public class KafkaConfigProducer
    {
        public string BootstrapServers { get; set; }
        public string Topic { get; set; }
    }
}
