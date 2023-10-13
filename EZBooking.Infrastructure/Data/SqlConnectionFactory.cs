using EZBooking.Application.Abstractions.Data;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZBooking.Infrastructure.Data
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string m_connectionString;
        public SqlConnectionFactory(string stConnectionString)
        {
            m_connectionString = stConnectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(m_connectionString);
            connection.Open();
            return connection;
        }
    }
}
