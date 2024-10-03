using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Data.SqlClient;

public class SqlServerHealthCheck : IHealthCheck
{
    private readonly string _connectionString;

    public SqlServerHealthCheck(string connectionString)
    {
        _connectionString = connectionString;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return Task.FromResult(HealthCheckResult.Healthy("SQL Server is reachable."));
            }
        }
        catch (SqlException)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("SQL Server is not reachable."));
        }
    }
}
