using Confluent.Kafka;
using Kafka_Project.Configurations;
using Kafka_Project.Interfaces;
using Kafka_Project.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Kafka_Project.Infra
{
    public class ConsumerKafka : IConsumerKafka
    {
        private readonly KafkaConfigConsumer _kafkaConfig;
        private readonly ConsumerConfig _consumerConfig;

        public ConsumerKafka(KafkaConfigConsumer config)
        {
            _kafkaConfig = config;

            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                GroupId = _kafkaConfig.GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true, // (the default)
                EnableAutoOffsetStore = false
            };            
        }

        public void GetMessage(Func<KafkaMessageConsumer, Task> action)
        {
            using (var consumer = new ConsumerBuilder<string, KafkaMessageConsumer>(_consumerConfig)
                .SetValueDeserializer(new Deserializer<KafkaMessageConsumer>())
                .Build())
            {
                consumer.Subscribe(_kafkaConfig.Topic);

                try
                {
                    var consumeResult = consumer.Consume();

                    //processar mensagem

                    if (consumeResult == null)
                    {
                        //TODO
                        //lançar exceção
                    }

                    Console.WriteLine(consumeResult.Message.Key);
                    Console.WriteLine(consumeResult.Message.Value);
                    Console.WriteLine(consumeResult.Offset);

                    var message = consumeResult.Message.Value;

                    action(message);

                    consumer.StoreOffset(consumeResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    
                }

                consumer.Close();
            }
        }

        public async Task SendDeadLetter(KafkaMessageConsumer message)
        {
            var _producerConfig = new ProducerConfig
            {
                BootstrapServers = _kafkaConfig.BootstrapServers,
                ClientId = Dns.GetHostName(),
                Acks = Acks.All
            };

            using (var deadLetter = new ProducerBuilder<string, KafkaMessageConsumer>(_producerConfig)
                .SetValueSerializer(new Serializer<KafkaMessageConsumer>())
                .Build())
            {
                var x = await deadLetter.ProduceAsync(_kafkaConfig.DeadLetter, new Message<string, KafkaMessageConsumer> { Key = "deadletter", Value = message, Timestamp = new Timestamp(DateTime.Now) });

                Console.WriteLine(x.Offset);
            }
        }
    }
}
