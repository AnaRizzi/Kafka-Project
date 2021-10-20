using Confluent.Kafka;
using Kafka_Project.Configurations;
using Kafka_Project.Interfaces;
using Kafka_Project.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Kafka_Project.Infra
{
    public class ConsumerKafka : IConsumerKafka
    {
        private readonly KafkaConfigConsumer _config;

        public ConsumerKafka(KafkaConfigConsumer config)
        {
            _config = config;
        }

        public void GetMessage(Action<KafkaMessageConsumer> action)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _config.BootstrapServers,
                GroupId = _config.GroupId,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true, // (the default)
                EnableAutoOffsetStore = false
            };

            using (var consumer = new ConsumerBuilder<string, string>(config).Build())
            {
                consumer.Subscribe(_config.Topic);

                var consumeResult = consumer.Consume();

                //processar mensagem
                try
                {
                    if (consumeResult != null)
                    {
                        Console.WriteLine(consumeResult.Message.Key);
                        Console.WriteLine(consumeResult.Message.Value);
                        Console.WriteLine(consumeResult.Offset);

                        var message = JsonSerializer.Deserialize<KafkaMessageConsumer>(consumeResult.Message.Value);
                        action(message);
                    }

                    consumer.StoreOffset(consumeResult);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                consumer.Close();
            }
        }
    }
}
