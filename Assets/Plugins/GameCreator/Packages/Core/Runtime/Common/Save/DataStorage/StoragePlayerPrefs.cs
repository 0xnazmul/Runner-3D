using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameCreator.Runtime.Common.SaveSystem
{
    [Serializable]
    public class StoragePlayerPrefs : IDataStorage
    {
        string IDataStorage.Title => "Player Prefs";
        string IDataStorage.Description => "Store all game information using Unity Player Prefs";

        // HIERARCHY: -----------------------------------------------------------------------------

        Task IDataStorage.DeleteAll()
        {
            PlayerPrefs.DeleteAll();
            return Task.FromResult(1);
        }

        Task IDataStorage.DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
            return Task.FromResult(1);
        }

        Task<bool> IDataStorage.HasKey(string key)
        {
            bool hasKey = PlayerPrefs.HasKey(key);
            return Task.FromResult(hasKey);
        }

        // GETTERS: -------------------------------------------------------------------------------

        public Task<object> GetBlob(string key, Type type, object value)
        {
            string json = PlayerPrefs.GetString(key, string.Empty);
            if (!string.IsNullOrEmpty(json)) value = JsonUtility.FromJson(json, type);

            return Task.FromResult(value);
        }

        Task<string> IDataStorage.GetString(string key, string value)
        {
            value = PlayerPrefs.GetString(key, value);
            return Task.FromResult(value);
        }

        Task<float> IDataStorage.GetFloat(string key, float value)
        {
            value = PlayerPrefs.GetFloat(key, value);
            return Task.FromResult(value);
        }

        Task<int> IDataStorage.GetInt(string key, int value)
        {
            value = PlayerPrefs.GetInt(key, value);
            return Task.FromResult(value);
        }

        // SETTERS: -------------------------------------------------------------------------------

        public Task SetBlob(string key, object value)
        {
            string json = JsonUtility.ToJson(value);

            PlayerPrefs.SetString(key, json);
            return Task.FromResult(1);
        }

        Task IDataStorage.SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
            return Task.FromResult(1);
        }

        Task IDataStorage.SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            return Task.FromResult(1);
        }

        Task IDataStorage.SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            return Task.FromResult(1);
        }
    }
}