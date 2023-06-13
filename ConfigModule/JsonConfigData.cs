using System;

namespace Core.ConfigModule
{
    [Serializable]
    public abstract class JsonConfigData<T> : ConfigData<T> where T : JsonConfigData<T>, new()
    {
        protected override string Ext => Extension.JSON;
    }
}