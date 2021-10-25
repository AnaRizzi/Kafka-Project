using Confluent.Kafka;
using Kafka_Project.Configurations;
using Kafka_Project.Interfaces;
using Kafka_Project.Models;
using System;
using System.Net;
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
            using (var producer = new ProducerBuilder<string, Message<KafkaMessageProducer>>(_producerConfig)
                .SetValueSerializer(new Serializer<Message<KafkaMessageProducer>>())
                .Build())
            {
                var x = await producer.ProduceAsync(_kafkaConfig.Topic, new Message<string, Message<KafkaMessageProducer>> { Key = message.Payload.Id.ToString(), Value = message, Timestamp = new Timestamp(DateTime.Now) });

                Console.WriteLine(x.Offset);
            }
        }        
    }
}
