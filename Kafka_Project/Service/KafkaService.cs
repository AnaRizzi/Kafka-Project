using Kafka_Project.Interfaces;
using Kafka_Project.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kafka_Project.Service
{
    public class KafkaService : IKafkaService
    {
        private readonly IConsumerKafka _consumer;
        private readonly IProducerKafka _producer;
        private readonly ILogger<KafkaService> _logger;


        public KafkaService(IConsumerKafka consumer, IProducerKafka producer, ILogger<KafkaService> logger)
        {
            _consumer = consumer;
            _producer = producer;
            _logger = logger;

        }
        public void ConsumeMessage()
        {
            //vai no Consumer para pegar a mensagem e passa a próxima ação como parâmetro para ser chamada do próprio consumer
            _consumer.GetMessage(ProcessMessage);
        }

        public async Task ProcessMessage(KafkaMessageConsumer message)
        {
            try
            {
                _logger.LogInformation("Iniciando o processamento da mensagem" + message.Id);

                var messageProducer = new KafkaMessageProducer(message);

                var payload = new Message<KafkaMessageProducer>(messageProducer);

                await _producer.Publish(payload);

                _logger.LogInformation("Mensagem publicada" + message.Id);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro ao processar mensagem, será enviada para deadletter. Id: " + message.Id + " Erro: " + ex.Message);

                await _consumer.SendDeadLetter(message);
            }
        }
    }
}
