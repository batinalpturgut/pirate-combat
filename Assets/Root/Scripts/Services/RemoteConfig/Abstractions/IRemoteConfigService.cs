using System;
using Root.Scripts.Services.Core.Attributes;

namespace Root.Scripts.Services.RemoteConfig.Abstractions
{
    [ServiceAPI]
    public interface IRemoteConfigService
    {
        bool IsRemoteConfigFetched { get; }
        event Action OnRemoteConfigFetched;
        bool TryGetValue(string key, out long value);
        bool TryGetValue(string key, out int value);
        bool TryGetValue(string key, out double value);
        bool TryGetValue(string key, out string value);
        bool TryGetValue(string key, out bool value);
    }
}