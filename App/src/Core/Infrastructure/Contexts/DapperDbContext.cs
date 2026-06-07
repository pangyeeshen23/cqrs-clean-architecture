using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Contexts
{
    public class DapperContext
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DapperContext(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found");
        }

        // notes : IDisposable is implemented into the IDbConnection, so we can use 'using' statement to ensure proper disposal of the connection
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);

    }
}
