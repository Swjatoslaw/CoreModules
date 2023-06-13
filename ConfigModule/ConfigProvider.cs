#if !UNITY_EDITOR
#define NOT_EDITOR
#endif

using System;
using System.Diagnostics;
using System.IO;
using Core.SingleService;
using Unity.Plastic.Newtonsoft.Json;

namespace Core.ConfigModule
{
    [Serializable]
    public abstract class ConfigProvider<T> where T : ConfigProvider<T>, new()
    {
        private static Func<T> getter;
        protected static T instance;

        static ConfigProvider()
        {
            getter = StaticConstructor;
        }

        private static T StaticConstructor()
        {
            ServiceManager.ApplicationPaused += Save;
            instance = new T();
            Load();

            return instance;
        }

        protected abstract string FullFileName { get; }

        protected abstract string FullPath { get; }

        protected abstract string Ext { get; }
        protected abstract ConfigName ConfigName { get; }
        protected virtual void OnLoad(){}

        public static void UpdateName(string newName)
        {
            if (instance.ConfigName.Name.Equals(newName) == false)
            {
                getter = Load;
                instance.ConfigName.Name = newName;
            }
        }
    
        protected static T Load()
        {
            var fullFileName = instance.FullFileName;
            
            if (File.Exists(fullFileName))
            {
                var json = File.ReadAllText(fullFileName);
                instance = JsonConvert.DeserializeObject<T>(json);
            }

            getter = GetInstance;
            instance.OnLoad();

            return instance;
        }

        private static T GetInstance()
        {
            return instance;
        }

        public static T Config => getter();

        public static void Save()
        {
            Set(instance);
        }
    
        public static void Set(T config)
        {
            if (Directory.Exists(config.FullPath) == false)
            {
                Directory.CreateDirectory(config.FullPath);
            }
            
            var json = Serialize(config);

            File.WriteAllText(instance.FullFileName,json);
        }
        
        private static string Serialize(T config)
        {
            var json = string.Empty;
            Serialize_Editor(config, ref json);
            Serialize_Runtime(config, ref json);

            return json;
        }
        
        [Conditional("UNITY_EDITOR")]
        private static void Serialize_Editor(T config, ref string json)
        {
            json = JsonConvert.SerializeObject(config, Formatting.Indented);
        }
        
        [Conditional("NOT_EDITOR")]
        private static void Serialize_Runtime(T config, ref string json)
        {
            json = JsonConvert.SerializeObject(config);
        }
    }
}