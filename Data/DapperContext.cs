using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Data
{
    using Microsoft.Extensions.Configuration;
    using System.Data;

    internal class DapperContext
    {
        private readonly string _connectionString;

        internal DapperContext(IConfiguration configuration)
        {
            _connectionString = configuration["MySettings:ConnectionString"];
        }

        internal IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
