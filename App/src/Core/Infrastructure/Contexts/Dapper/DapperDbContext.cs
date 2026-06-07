using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Contexts.Dapper
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(IOptions<DapperSetting> settings)
        {
            _connectionString = settings.Value.ConnectionString ?? throw new ArgumentNullException(nameof(settings));
        }

        // notes : IDisposable is implemented into the IDbConnection, so we can use 'using' statement to ensure proper disposal of the connection
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

    }
}
