using Kafka_Project.Interfaces;
using Kafka_Project.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.Service
{
    public class KafkaService : IKafkaService
    {
        private readonly IConsumerKafka _consumer;
        private readonly IProducerKafka _producer;

        public KafkaService(IConsumerKafka consumer, IProducerKafka producer)
        {
            _consumer = consumer;
            _producer = producer;
        }
        public void ConsumeMessage()
        {
            //vai no Consumer para pegar a mensagem e passa a próxima ação como parâmetro para ser chamada do próprio consumer
            _consumer.GetMessage(ProcessMessage);
        }

        public void ProcessMessage(KafkaMessageConsumer message)
        {
            Console.WriteLine(message.Id);
        }
    }
}
