using System;
using UnityEngine;

namespace Core.ConfigModule
{
    [Serializable]
    public abstract class ConfigData<T> : ConfigProvider<T> where T : ConfigData<T>, new()
    {
#if !UNITY_EDITOR
        protected override string FullPath => System.IO.Path.Combine(Application.persistentDataPath, FolderName.Configs, Path);
#else
        protected override string FullPath => System.IO.Path.Combine(Application.dataPath, FolderName.CONFIGS, Path);
#endif
        protected virtual string Path => GetType().Name;
        protected override string FullFileName => System.IO.Path.Combine(FullPath, $"{ConfigName.Name}.{Ext}");
    }
}