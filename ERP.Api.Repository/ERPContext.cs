using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace ERP.Api.Repository;

public class ERPContext
{
    private readonly IConfiguration _configuration;
    private readonly string? _connectionString;

    public ERPContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection");
    }

    public IDbConnection CreateConnection() => new MySqlConnection(_connectionString);
}
