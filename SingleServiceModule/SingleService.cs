using System;
using UnityEngine;

namespace Core.SingleService
{
    public abstract class SingleService<T> : BaseSingleService where T : SingleService<T>
    {
        private static T instance;
        private static Func<T> staticConstructor;
        private static Action onInitializing;
        public override Type Type => typeof(T);

        protected static T Instance
        {
            get
            { 
                return staticConstructor();
            }
            set
            {
                instance = value;
            }
        }

        static SingleService()
        {
            staticConstructor = StaticConstructor;
        }

        private static T StaticConstructor()
        {
            onInitializing?.Invoke();
            staticConstructor = ThrowException;

            if (instance == null)
            {
                instance = Instantiate(ServiceManager.GetService<T>());
            }

            staticConstructor = GetInstance;
            
            instance.Init();

            return instance;
        }

        private static T GetInstance() => instance;
        private static T ThrowException() => throw new Exception($"[{typeof(T)}] You try get {nameof(Instance)} before initializing. Use {nameof(Init)} method by override in {typeof(T)} class.");

        protected virtual void Awake()
        {
            Debug.Log($"[{GetType().Name}] Awake");
        }

        protected virtual void Init() { }

        protected static void OnInitializing(Action action)
        {
            onInitializing = action;
        }
    
        protected static void AddOnInitializing(Action action)
        {
            onInitializing += action;
        }
    }
}