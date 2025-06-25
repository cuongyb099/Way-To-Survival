using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace KatLib.Data_Serialize
{
    [DefaultExecutionOrder(100)]
    public static class DataSerialize
    {
        public static string SavePath { get; private set; }
        public static Action OnSave;
        private static SaveConfig _saveConfig;
        private static IEncryptionModule _encryptionModuleModule;
        private static ISerializeModule _serializeModule;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadSaveFile()
        {
            _saveConfig = Resources.Load<SaveConfig>("SaveConfig");
            SetUpEncryptionModule();
            SetUpSerializeModule();
            
            SavePath = GenerateSavePath();
            _serializeModule.LoadFile(SavePath);
#if UNITY_EDITOR
                AssetDatabase.Refresh();
#endif
        }

        private static void SetUpSerializeModule()
        {
            switch (_saveConfig.SerializeType)
            {
                case SerializeType.Json:
                    _serializeModule = new JsonSerialize(_encryptionModuleModule, _saveConfig);
                    break;
            }
        }

        private static void SetUpEncryptionModule()
        {
            switch (_saveConfig.EncryptionType)
            {
                case EncryptionType.None:
                    break;
                case EncryptionType.AES:
                    _encryptionModuleModule = new AesEncryption();
                    break;
                case EncryptionType.DES:
                    _encryptionModuleModule = new DesEncryptionModule();
                    break;
            }
        }

        public static void SaveFile()
        {
            OnSave?.Invoke();
            _serializeModule.SaveFile(SavePath);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static async UniTask SaveFileAsync()
        {
            OnSave?.Invoke();
            await UniTask.RunOnThreadPool(() => _serializeModule.SaveFile(SavePath));
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }

        public static bool TryGetData<T>(string key, out T result) where T : new()
        {
            var data = _serializeModule.GetData<T>(key);
            if (data == null)
            {
                result = new T();
                return false;
            }
            
            result = data;
            return true;
        }
        
        public static T GetData<T>(string key) => _serializeModule.GetData<T>(key);

        public static void SetData<T>(string key, T value) => _serializeModule.SetData(key, value);

        public static void DeleteSave()
        {
            _serializeModule.Clear();
            File.Delete(SavePath);
        }

        public static async UniTask DeleteSaveAsync()
        {
            await UniTask.RunOnThreadPool(() =>
            {
                _serializeModule.Clear();
                File.Delete(SavePath);
            });
        }
        
        private static string GenerateSavePath()
        {
#if UNITY_EDITOR
            SavePath = "Assets/Save In Editor/" + _saveConfig.FileName;
#else
            SavePath = Path.Combine(Application.persistentDataPath, _saveConfig.FileName);
#endif
            switch (_saveConfig.SerializeType)
            {
                case SerializeType.Json:
                    return SavePath += ".json";
                case SerializeType.Binary:
                    return SavePath += ".sav";
            }

            return null;
        }
    }
}
