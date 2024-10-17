using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
using Polly.Timeout;

public class ProductServiceHealthCheck : IHealthCheck
{
    private readonly HttpClient _httpClient;
    private readonly IAsyncPolicy<HttpResponseMessage> _policy;

    public ProductServiceHealthCheck(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // Định nghĩa chính sách retry
        var retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .RetryAsync(3); // Thử lại 3 lần

        // Định nghĩa chính sách circuit breaker
        var circuitBreakerPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1)); // Ngắt mạch nếu thất bại 2 lần liên tiếp trong 1 phút

        // Định nghĩa chính sách timeout
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(30));

        // Kết hợp các chính sách lại với nhau
        _policy = Policy.WrapAsync(timeoutPolicy, retryPolicy, circuitBreakerPolicy);
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _policy.ExecuteAsync(() => CheckProductServiceHealthAsync());

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("Product Service is healthy.");
            }
            else
            {
                return HealthCheckResult.Unhealthy("Product Service is unhealthy.");
            }
        }
        catch (BrokenCircuitException)
        {
            return HealthCheckResult.Unhealthy("Product Service circuit is open.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Product Service health check failed: {ex.Message}");
        }
    }

    private async Task<HttpResponseMessage> CheckProductServiceHealthAsync()
    {
        return await _httpClient.GetAsync("/api/Product/health");
    }
}
