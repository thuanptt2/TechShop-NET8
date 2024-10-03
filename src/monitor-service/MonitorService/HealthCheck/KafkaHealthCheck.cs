using Confluent.Kafka;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public class KafkaHealthCheck : IHealthCheck
{
    private readonly string _kafkaBootstrapServers;

    public KafkaHealthCheck(string kafkaBootstrapServers)
    {
        _kafkaBootstrapServers = kafkaBootstrapServers;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var config = new ProducerConfig 
            {
                BootstrapServers = _kafkaBootstrapServers,
                MessageTimeoutMs = 5000
            };
            
            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                // Kiểm tra kết nối đến Kafka (sử dụng ProduceAsync để kiểm tra phản hồi)
                var result = await producer.ProduceAsync("healthcheck_topic", new Message<Null, string> { Value = "ping" });

                return HealthCheckResult.Healthy("Kafka is reachable.");
            }
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Kafka is not reachable.", ex);
        }
    }
}
