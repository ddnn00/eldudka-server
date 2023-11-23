using Npgsql;
using System.Data;

namespace EldudkaServer.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration["pgSqlConnectionString"];
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
    }
}
