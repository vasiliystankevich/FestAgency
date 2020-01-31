using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using DataGenerator.Interfaces;
using log4net;
using Logs;
using Project.Kernel;

namespace DataGenerator
{
    public class DataLoader:IDataLoader
    {
        public DataLoader(ISaContext saContext, ILogsContext logsContext, IWrapper<ILog> log, IExecutor executor)
        {
            SaContext = saContext;
            LogsContext = logsContext;
            Log = log;
            Executor = executor;
        }

        public void RecreateDb()
        {
            var logsSqlDbName = ConfigurationManager.AppSettings["LogsSqlDbName"];
            var logsSqlDbStartSize = ConfigurationManager.AppSettings["LogsSqlDbStartSize"];
            var sqlDbUser = ConfigurationManager.AppSettings["SqlDbUser"];
            var sqlDbUserPassword = ConfigurationManager.AppSettings["SqlDbUserPassword"];

            Log.Instance.Info("Get path to db files");
            var pathToDbFiles = GetPathToDbFiles();
            Log.Instance.Info("Recreate login for database server");
            ExecuteDeploymentScriptsNoResult("CreateLoginForDbServer.sql", sqlDbUser, sqlDbUserPassword);
            Log.Instance.Info($"Recreate database: {logsSqlDbName}");
            ExecuteDeploymentScriptsNoResult("RecreateDb.sql", pathToDbFiles, logsSqlDbName, logsSqlDbStartSize, sqlDbUser);
        }

        public void LogsDbMigrations()
        {
            var logsSqlDbName = ConfigurationManager.AppSettings["LogsSqlDbName"];
            Log.Instance.Info($"Execute migrations for database: {logsSqlDbName}");
            ExecuteDeploymentScriptsNoResult("LogsMigrations.sql", logsSqlDbName);
        }

        public void ExecuteDeploymentScriptsNoResult(string templateSqlFileName, params string[] args)
        {
            var pathToFolderSqlScript = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlScripts");
            var pathToTemplateFileScript = Path.Combine(pathToFolderSqlScript, templateSqlFileName);
            var sqlTemplateScript = File.ReadAllText(pathToTemplateFileScript);
            var sqlScript = string.Format(sqlTemplateScript, args);
            SaContext.ExecuteServerScriptNoResult(sqlScript);
        }

        public List<TResult> ExecuteDeploymentScriptsScalar<TResult>(string templateSqlFileName, params string[] args)
        {
            var pathToFolderSqlScript = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SqlScripts");
            var pathToTemplateFileScript = Path.Combine(pathToFolderSqlScript, templateSqlFileName);
            var sqlTemplateScript = File.ReadAllText(pathToTemplateFileScript);
            var sqlScript = string.Format(sqlTemplateScript, args);
            return SaContext.ExecuteServerScript<TResult>(sqlScript);
        }

        public void Seed()
        {
            Executor.ExecuteAction(RecreateDb);
            Executor.ExecuteAction(LogsDbMigrations);
        }

        protected string GetPathToDbFiles()
        {
            var pathToMasterDbFile = ExecuteDeploymentScriptsScalar<string>("GetPathToMasterDbFiles.sql").First();
            return Path.GetDirectoryName(pathToMasterDbFile);
        }

        ILogsContext LogsContext { get; }
        ISaContext SaContext { get; }
        IExecutor Executor { get; }
        IWrapper<ILog> Log { get; }
    }
}
