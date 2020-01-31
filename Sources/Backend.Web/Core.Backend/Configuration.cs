using System;
using System.Web.Configuration;
using Backend.Web.Core.Backend.Interfaces;

namespace Backend.Web.Core.Backend
{
    public class Configuration: IConfiguration
    {
        public T GetValue<T>(string key, Func<string, T> transform) =>
            transform(WebConfigurationManager.AppSettings[key]);

        public string GetString(string key) => GetValue(key, value => value);
        public int GetInt(string key) => GetValue(key, Convert.ToInt32);
        public Guid GetGuid(string key) => GetValue(key, Guid.Parse);
        public TimeSpan GeTimeSpan(string key) => TimeSpan.FromSeconds(GetInt(key));
    }
}