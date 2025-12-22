#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Root.Scripts
{
    [InitializeOnLoad]
    public class SceneRedirector
    {
        private static readonly List<SceneRedirectRule> RedirectRules = new List<SceneRedirectRule>
        {
            new SceneRedirectRule()
            {
                Source = Path("TowerDefenseScene"),
                Target = Path("MainScene")
            }
        };

        private static string SourceScenePath
        {
            get => SessionState.GetString(nameof(SourceScenePath), string.Empty);
            set => SessionState.SetString(nameof(SourceScenePath), value);
        }

        private static bool RedirectionCompleted
        {
            get => SessionState.GetBool(nameof(RedirectionCompleted), false);
            set => SessionState.SetBool(nameof(RedirectionCompleted), value);
        }

        private class SceneRedirectRule
        {
            public string Source { get; set; }
            public string Target { get; set; }
        }

        private static string Path(string sceneName)
        {
            return $"Assets/Root/Scenes/{sceneName}.unity";
        }

        // https://stackoverflow.com/questions/35586103/unity3d-load-a-specific-scene-on-play-mode
        static SceneRedirector()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            string currentScene = SceneManager.GetActiveScene().path;

            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                {
                    RedirectionCompleted = false;
                    foreach (SceneRedirectRule rule in RedirectRules)
                    {
                        if (rule.Source == currentScene)
                        {
                            SourceScenePath = currentScene;
                            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                            EditorSceneManager.OpenScene(rule.Target);
                            RedirectionCompleted = true;
                            Debug.LogWarning($"{rule.Source} redirected to {rule.Target}");
                            break;
                        }
                    }

                    break;
                }
                case PlayModeStateChange.EnteredEditMode:
                {
                    if (RedirectionCompleted && !string.IsNullOrEmpty(SourceScenePath))
                    {
                        EditorSceneManager.OpenScene(SourceScenePath);
                    }

                    break;
                }
            }
        }
    }
}
#endif