using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KatInventory;
using Newtonsoft.Json;
using UnityEditor;

namespace Tech.Json
{
    public static class Json 
    {
        private static readonly string _key = "gCjK+DZ/GCYbKIGiAt1qCA==";
        private static readonly string _iv = "47l5QsSe1POo31adQ/u7nQ==";
        private static readonly JsonSerializerSettings settings = new() { TypeNameHandling = TypeNameHandling.All };
        
        //Kat Note : If You Want To See Raw File Just Command = new AES(_key, _iv)
        private static IEncryption _encryption /* = new AES(_key, _iv);*/;

        public static void SaveJson<T>(this T data, string path)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            WriteAllText(path, json);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
    
        public static async void SaveJsonAsync<T>(this T data, string path, Action saveDone = null)
        {
            await Task.Run(() =>
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented,settings);
                WriteAllText(path, _encryption.Encrypt(json));
            });

            saveDone?.Invoke();
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        
        public static void LoadJson<T>(string path, out T value)
        {
            if (File.Exists(path))
            {
                string json = ReadAllText(path);
                T data = JsonConvert.DeserializeObject<T>(json, settings);
                value = data;
                return;
            }

            value = default;
        }

        private static void WriteAllText(string path, string text)
        {
            if (_encryption != null)
            {
                File.WriteAllText(path, JsonConvert.SerializeObject(_encryption.Encrypt(text), Formatting.Indented));
                return;
            }

            File.WriteAllText(path, text);
        }
        
        
        public static string ReadAllText(string path)
        {
            if (_encryption != null)
            {
                return _encryption.Decrypt(JsonConvert.DeserializeObject<string>(File.ReadAllText(path)));
            }

            return File.ReadAllText(path);
        }
    }
}