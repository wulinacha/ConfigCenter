using ConfigCenter.Repository.Implement;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ConfigCenter.Repository
{
    public static class RepositoryExtensions
    {
        public static void UseRepository(string connectionString) {
            Database.ormLiteConnectionFactory = new OrmLiteConnectionFactory(connectionString, MySqlDialect.Provider);
        }

    }
}
