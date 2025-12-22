using System.Collections;
using UnityEngine;

namespace Root.Scripts.Utilities
{
    public class StaticCoroutineRunner : MonoBehaviour
    {
        private static StaticCoroutineRunner _instance;

        private static StaticCoroutineRunner Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                GameObject coroutineRunner = new GameObject(nameof(StaticCoroutineRunner))
                {
                    hideFlags = HideFlags.HideInHierarchy
                };

                _instance = coroutineRunner.AddComponent<StaticCoroutineRunner>();

                DontDestroyOnLoad(coroutineRunner);
                return _instance;
            }
        }

        public static void DirectStartCoroutine(IEnumerator coroutine)
        {
            Instance.StartCoroutine(coroutine);
        }

        public static void DirectStopCoroutine(IEnumerator coroutine)
        {
            Instance.StopCoroutine(coroutine);
        }

        public static void DirectStopAllCoroutine()
        {
            Instance.StopAllCoroutines();
        }
    }
}