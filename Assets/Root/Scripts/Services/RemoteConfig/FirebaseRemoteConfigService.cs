using System;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Root.Scripts.Services.Core.Abstractions;
using Root.Scripts.Services.RemoteConfig.Abstractions;
using Root.Scripts.Utilities.Guards;
using Root.Scripts.Utilities.Logger;
using Root.Scripts.Utilities.Logger.Enums;
using UnityEngine;
using LogType = Root.Scripts.Utilities.Logger.Enums.LogType;

namespace Root.Scripts.Services.RemoteConfig
{
    [CreateAssetMenu(fileName = "FirebaseRemoteConfigService", menuName = "Services/RemoteConfig/FirebaseRemoteConfigService", order = 0)]
    public class FirebaseRemoteConfigService : AService, IRemoteConfigService
    {
        [SerializeField, Range(1, 1440)] public int minimumFetchIntervalInMinutes = 300;

        [SerializeField, Range(5, 3600)] public int fetchTimeoutInSeconds = 5;

        public bool IsRemoteConfigFetched { get; private set; }
        public event Action OnRemoteConfigFetched;

        protected override void OnInitialize()
        {
            FirebaseApp.CheckAndFixDependenciesAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        DependencyStatus dependencyStatus = task.Result;
                        if (dependencyStatus == DependencyStatus.Available)
                        {
                            InitializeRemoteConfig();
                        }
                        else
                        {
                            Log.Console($"Firebase dependencies could not be resolved: {dependencyStatus}",
                                LogType.Error,
                                LogContext.Always, Prefix.GameServices);
                        }
                    }
                    else
                    {
                        Log.Console("An error occurred while checking Firebase dependencies.", LogType.Error,
                            LogContext.Always, Prefix.GameServices);
                    }
                });
        }

        private void InitializeRemoteConfig()
        {
            ConfigSettings remoteConfigSettings = new ConfigSettings
            {
                FetchTimeoutInMilliseconds =
                    Convert.ToUInt64(TimeSpan.FromSeconds(fetchTimeoutInSeconds).TotalMilliseconds),
                MinimumFetchIntervalInMilliseconds =
                    Convert.ToUInt64(TimeSpan.FromMinutes(minimumFetchIntervalInMinutes).TotalMilliseconds)
            };

            FirebaseRemoteConfig.DefaultInstance.SetConfigSettingsAsync(remoteConfigSettings)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        Log.Console("Firebase Remote Config initialized.", LogType.Debug, LogContext.Always,
                            Prefix.GameServices);
                        IsInitialized = true;
                    }
                    else
                    {
                        Log.Console("An error occurred while configuring Firebase Remote Config settings.",
                            LogType.Error, LogContext.Always, Prefix.GameServices);
                    }
                });
        }

        protected override void OnStart()
        {
            FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync()
                .ContinueWithOnMainThread(task =>
                {
                    if (task.Result)
                    {
                        Log.Console("Firebase Remote Config fetch and activate succeeded.", LogType.Debug,
                            LogContext.Always, Prefix.GameServices);
                        IsRemoteConfigFetched = true;
                        OnRemoteConfigFetched?.Invoke();
                    }
                    else
                    {
                        Log.Console("Firebase Remote Config fetch failed.", LogType.Error,
                            LogContext.Always, Prefix.GameServices);
                        IsRemoteConfigFetched = false;
                        OnRemoteConfigFetched?.Invoke();
                    }
                });
        }

        public bool TryGetValue(string key, out long value)
        {
            value = default;

            Guard.Against.Null(FirebaseRemoteConfig.DefaultInstance, nameof(FirebaseRemoteConfig.DefaultInstance));

            ConfigValue configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);

            if (configValue.Source == ValueSource.DefaultValue)
            {
                return false;
            }

            try
            {
                value = configValue.LongValue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValue(string key, out double value)
        {
            value = default;

            Guard.Against.Null(FirebaseRemoteConfig.DefaultInstance, nameof(FirebaseRemoteConfig.DefaultInstance));

            ConfigValue configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);

            if (configValue.Source == ValueSource.DefaultValue)
            {
                return false;
            }

            try
            {
                value = configValue.DoubleValue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValue(string key, out bool value)
        {
            value = default;

            Guard.Against.Null(FirebaseRemoteConfig.DefaultInstance, nameof(FirebaseRemoteConfig.DefaultInstance));

            ConfigValue configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);

            if (configValue.Source == ValueSource.DefaultValue)
            {
                return false;
            }

            try
            {
                value = configValue.BooleanValue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValue(string key, out string value)
        {
            value = default;

            Guard.Against.Null(FirebaseRemoteConfig.DefaultInstance, nameof(FirebaseRemoteConfig.DefaultInstance));

            ConfigValue configValue = FirebaseRemoteConfig.DefaultInstance.GetValue(key);

            if (configValue.Source == ValueSource.DefaultValue || string.IsNullOrEmpty(configValue.StringValue))
            {
                return false;
            }

            try
            {
                value = configValue.StringValue;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetValue(string key, out int value)
        {
            value = default;

            if (!TryGetValue(key, out long longValue))
            {
                return false;
            }
            
            value = (int)longValue;
            return true;

        }
    }
}