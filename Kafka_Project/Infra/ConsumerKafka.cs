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

        public void GetMessage(Action<KafkaMessageConsumer> action)
        {
            using (var consumer = new ConsumerBuilder<string, string>(_consumerConfig).Build())
            {
                consumer.Subscribe(_kafkaConfig.Topic);

                try
                {
                    var consumeResult = consumer.Consume();

                    //processar mensagem
                
                    if (consumeResult == null)
                    {
                        //lançar exceção
                    }

                    Console.WriteLine(consumeResult.Message.Key);
                    Console.WriteLine(consumeResult.Message.Value);
                    Console.WriteLine(consumeResult.Offset);

                    var message = JsonSerializer.Deserialize<KafkaMessageConsumer>(consumeResult.Message.Value);
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
    }
}
