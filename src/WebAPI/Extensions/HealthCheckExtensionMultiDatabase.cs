using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace WebApi.Extensions
// can be moved to infrastructure project, so the webapi does not deal with dapper
{
    public class HealthCheckExtensionMultiDatabase : IHealthCheck
    {
        private readonly IDictionary<string, (string connectionString, IList<string> Queries, string Provider)> _databasesQueries;

        public HealthCheckExtensionMultiDatabase(IDictionary<string, (string connectionString, IList<string> Queries, string provider)> databasesQueries)
        {
            _databasesQueries = databasesQueries;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResults = new Dictionary<string, HealthCheckResult>();

            foreach (var dbQuery in _databasesQueries)
            {
                foreach (var query in dbQuery.Value.Queries)
                {
                    try
                    {
                        using (var connection = CreateConnection(dbQuery.Value.connectionString, dbQuery.Value.Provider))
                        {
                            await connection.OpenAsync(cancellationToken);

                            using (var command = CreateCommand(query, connection, dbQuery.Value.Provider))
                            {
                                await command.ExecuteScalarAsync(cancellationToken);
                                healthCheckResults[$"{dbQuery.Key}-{query}"] = HealthCheckResult.Healthy("ok");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        var errorMessage = Regex.Unescape($"Exception during query execution: {ex.Message}");
                        healthCheckResults[$"{dbQuery.Key}-{query}"] = HealthCheckResult.Unhealthy(errorMessage);
                    }
                }
            }

            var overallStatus = healthCheckResults.Values.All(result => result.Status == HealthStatus.Healthy) ? HealthStatus.Healthy : HealthStatus.Unhealthy;
            return new HealthCheckResult(overallStatus, description: "Multi-database health check", data: healthCheckResults.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value.Description!)!);
        }

        private DbConnection CreateConnection(string connectionString, string provider)
        {
            return provider switch
            {
                "SqlServer" => new SqlConnection(connectionString),
                "Oracle" => new OracleConnection(connectionString),
                _ => throw new NotSupportedException($"Provider {provider} is not supported.")
            };
        }

        private DbCommand CreateCommand(string query, DbConnection connection, string provider)
        {
            return provider switch
            {
                "SqlServer" => new SqlCommand(query, (SqlConnection)connection),
                "Oracle" => new OracleCommand(query, (OracleConnection)connection),
                _ => throw new NotSupportedException($"Provider {provider} is not supported.")
            };
        }
    }
}
