using System;
using System.Collections.Generic;
using Root.Scripts.Utilities.Abstractions;
using Root.Scripts.Utilities.Logger;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts
{
    public abstract class AContext : MonoBehaviour
    {
        private static readonly HashSet<AContext> SharedContexts = new HashSet<AContext>();
        private readonly Dictionary<Type, object> _references = new Dictionary<Type, object>();
        protected abstract bool IsPersistent { get; }

        protected void Register<T>(T instance, bool keepParent = false) where T : class
        {
            _references[typeof(T)] = instance;

            if (!keepParent && instance is MonoBehaviour mbInstance)
            {
                mbInstance.transform.SetParent(transform);
            }
        }

        protected void Register(object instance, Type type, bool keepParent = false)
        {
            _references[type] = instance;

            if (!keepParent && instance is MonoBehaviour mbInstance)
            {
                mbInstance.transform.SetParent(transform);
            }
        }

        public void Unregister<T>(T instance)
        {
            if (_references.ContainsKey(typeof(T)))
            {
                _references.Remove(typeof(T));
            }
        }

        protected void Init<T>(T instance, AContext context) where T : class, IInitializer<AContext>
        {
            if (instance is IInitializer<AContext> initializable)
            {
                initializable.Initialize(context);
            }
        }

        public T Resolve<T>(bool suppressErrorMessages = false) // Service Locator Pattern
        {
            if (_references.TryGetValue(typeof(T), out var instance))
            {
                if (instance != null)
                {
                    return (T)instance;
                }

                _references.Remove(typeof(T));
            }

            if (!suppressErrorMessages)
            {
                Log.Console($"{typeof(T)} instance not found in context!", LogType.Error);
            }

            return default;
        }

        private void RegisterShared(AContext context)
        {
            SharedContexts.Add(context);
        }

        private void UnregisterShared(AContext context)
        {
            SharedContexts.Remove(context);
        }

        public T ResolveShared<T>(bool suppressErrorMessages = false)
        {
            foreach (AContext context in SharedContexts)
            {
                T resolvedService = context.Resolve<T>(true);
                if (resolvedService != null)
                {
                    return resolvedService;
                }
            }
            
            if (!suppressErrorMessages)
            {
                Log.Console($"{typeof(T)} instance not found in shared context!", LogType.Error);
            }

            return default;
        }

        protected abstract void RegisterServices();
        protected abstract void InitServices();

        private void Awake()
        {
            if (IsPersistent)
            {
                DontDestroyOnLoad(gameObject);
                RegisterShared(this);
            }

            RegisterServices();
            InitServices();
        }

        private void OnDestroy()
        {
            if (IsPersistent)
            {
                UnregisterShared(this);
            }
        }
    }
}