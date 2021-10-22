using Confluent.Kafka;
using Kafka_Project.Configurations;
using Kafka_Project.Interfaces;
using Kafka_Project.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kafka_Project.Infra
{
    public class ProducerKafka : IProducerKafka
    {
        private readonly KafkaConfigProducer _kafkaConfig;
        private readonly ProducerConfig _producerConfig;

        public ProducerKafka(KafkaConfigProducer config)
        {
            _kafkaConfig = config;

            _producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                ClientId = Dns.GetHostName(),
                Acks = Acks.All
            };
        }

        public async Task Publish(Message<KafkaMessageProducer> message)
        {
            var jsonObject = JsonSerializer.Serialize(message);

            using (var producer = new ProducerBuilder<string, string>(_producerConfig).Build())
            {
                var x = await producer.ProduceAsync(_kafkaConfig.Topic, new Message<string, string> { Key = message.Payload.Id.ToString(), Value = jsonObject, Timestamp = new Timestamp(DateTime.Now) });

                Console.WriteLine(x.Offset);
            }
        }        
    }
}
