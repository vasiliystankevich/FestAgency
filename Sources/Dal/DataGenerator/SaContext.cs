using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using DataGenerator.Interfaces;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace DataGenerator
{
    public class SaContext: DbContext, ISaContext
    {
        public SaContext() : base("SaContextConnection") { }
        public void ExecuteTransaction(Action action)
        {
            using (var transaction = Database.BeginTransaction())
            {
                try
                {
                    action();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public void ExecuteServerScriptNoResult(string script)
        {
            var serverConnection = new ServerConnection((SqlConnection)Database.Connection);
            var server = new Server(serverConnection);
            server.ConnectionContext.ExecuteNonQuery(script);
        }

        public List<TResult> ExecuteServerScript<TResult>(string script)
        {
            var resultQuery = Database.SqlQuery<TResult>(script);
            return resultQuery.ToList();
        }
    }
}
