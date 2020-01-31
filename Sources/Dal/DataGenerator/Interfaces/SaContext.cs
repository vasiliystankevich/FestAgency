using System.Collections.Generic;
using Project.Kernel.Dal;

namespace DataGenerator.Interfaces
{
    public interface ISaContext : IDbContext
    {
        void ExecuteServerScriptNoResult(string script);
        List<TResult> ExecuteServerScript<TResult>(string script);
    }
}