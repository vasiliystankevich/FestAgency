using System;

namespace Backend.Web.Core.Backend.Interfaces
{
    public interface IConfiguration
    {
        T GetValue<T>(string key, Func<string, T> transform);
        string GetString(string key);
        int GetInt(string key);
        Guid GetGuid(string key);
        TimeSpan GeTimeSpan(string key);
    }
}
