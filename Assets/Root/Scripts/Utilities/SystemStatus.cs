using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Root.Scripts.Utilities
{
    [DefaultExecutionOrder(-9999)]
    public sealed class SystemStatus : MonoBehaviour
    {
        private static SystemStatus _instance;
        private static SystemStatus Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                GameObject systemStatus = new GameObject(nameof(SystemStatus))
                {
                    hideFlags = HideFlags.HideInHierarchy
                };

                _instance = systemStatus.AddComponent<SystemStatus>();

                DontDestroyOnLoad(systemStatus);
                return _instance;
            }
        }

        public static bool IsApplicationQuitting => Instance._isApplicationQuitting;
        public static bool IsApplicationPaused => Instance._isApplicationPaused;
        public static int FPS => Instance._currentAveraged;
        public static string FPSFormatted => Instance._fpsFormatted;

        private bool _isApplicationQuitting;
        private bool _isApplicationPaused;

        private const FPSDeltaTime DeltaTimeType = FPSDeltaTime.Smooth;

        private readonly Dictionary<int, string> _cachedNumberStrings = new Dictionary<int, string>();
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        private int[] _frameRateSamples;
        private readonly int _cacheNumbersAmount = 300;
        private readonly int _averageFromAmount = 30;
        private int _averageCounter;
        private int _currentAveraged;
        private string _fpsFormatted;

        private void Awake()
        {
            for (int i = 0; i < _cacheNumbersAmount; i++)
            {
                _cachedNumberStrings[i] = i.ToString();
            }

            _frameRateSamples = new int[_averageFromAmount];
        }

        // https://gist.github.com/st4rdog/80057b406bfd00f44c8ec8796a071a13
        private void Update()
        {
            int currentFrame = (int)Math.Round(1f / DeltaTimeType switch
            {
                FPSDeltaTime.Smooth => Time.smoothDeltaTime,
                FPSDeltaTime.Unscaled => Time.unscaledDeltaTime,
                _ => Time.unscaledDeltaTime
            });
            _frameRateSamples[_averageCounter] = currentFrame;

            float average = 0f;

            foreach (int frameRate in _frameRateSamples)
            {
                average += frameRate;
            }

            _currentAveraged = (int)Math.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;

            if (_currentAveraged >= 0 && _currentAveraged < _cacheNumbersAmount)
            {
                _fpsFormatted = _cachedNumberStrings[_currentAveraged];
            }
            else if (_currentAveraged >= _cacheNumbersAmount)
            {
                _stringBuilder.Clear();
                _stringBuilder.Append("> ");
                _stringBuilder.Append(_cacheNumbersAmount);
                _fpsFormatted = _stringBuilder.ToString();
            }
            else if (_currentAveraged < 0)
            {
                _fpsFormatted = "< 0";
            }
            else
            {
                _fpsFormatted = "?";
            }
        }

        private void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _isApplicationPaused = pauseStatus;
        }

        private enum FPSDeltaTime
        {
            Smooth,
            Unscaled
        }
    }
}