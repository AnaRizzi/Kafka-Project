using Kafka_Project.Configurations;
using Kafka_Project.Infra;
using Kafka_Project.Interfaces;
using Kafka_Project.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka_Project.IoC
{
    public static class IoC
    {
        private const string KAFKA_CONSUMER = "KafkaConsumer";
        private const string KAFKA_PRODUCER = "KafkaProducer";

        public static IServiceCollection ConfigureContainer(this IServiceCollection services, IConfiguration configuration)
        {
            //TODO
            services.AddSingleton<IKafkaService, KafkaService>();
            services.AddSingleton<IProducerKafka, ProducerKafka>();
            services.AddSingleton(configuration.GetSection(KAFKA_PRODUCER).Get<KafkaConfigProducer>());
            services.AddSingleton<IConsumerKafka, ConsumerKafka>();
            services.AddSingleton(configuration.GetSection(KAFKA_CONSUMER).Get<KafkaConfigConsumer>());

            return services;
        }
    }
}
