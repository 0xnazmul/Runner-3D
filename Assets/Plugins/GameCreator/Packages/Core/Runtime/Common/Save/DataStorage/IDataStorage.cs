using System;
using System.Threading.Tasks;

namespace GameCreator.Runtime.Common.SaveSystem
{
    public interface IDataStorage
    {
        string Title { get; }
        string Description { get; }

        Task DeleteAll();

        Task DeleteKey(string key);
        Task<bool> HasKey(string key);

        Task<object> GetBlob(string key, Type type, object value);
        Task SetBlob(string key, object value);

        Task<string> GetString(string key, string value);
        Task SetString(string key, string value);

        Task<float> GetFloat(string key, float value);
        Task SetFloat(string key, float value);

        Task<int> GetInt(string key, int value);
        Task SetInt(string key, int value);
    }
}