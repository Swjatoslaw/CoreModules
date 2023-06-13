using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace Core.SingleService
{
    public class ServiceManager : MonoBehaviour
    {
        public static event Action ApplicationPaused;
        [SerializeField] private List<BaseSingleService> services;
        private static readonly Dictionary<Type, BaseSingleService> Services = new ();

        private void Awake()
        {
            DontDestroyOnLoad(this);
            Debug.Log("[ServiceManager] Awake");

            for (int i = 0; i < services.Count; i++)
            {
                var service = services[i];

                Services.Add(service.Type, service);
            }
        }

        public static T GetService<T>() where T : BaseSingleService
        {
            return (T)Services[typeof(T)];
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                ApplicationPaused?.Invoke();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            OnApplicationPause(!hasFocus);
        }
    }
}
