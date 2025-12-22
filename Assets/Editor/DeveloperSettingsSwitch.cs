using System.Collections.Generic;
using Root.Scripts.Utilities;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Editor
{
    public class DeveloperSettingsSwitch : EditorWindow, IPreprocessBuildWithReport
    {
        private const string MenuPath = "DevSettings/DevModes/";
        
        private const string SwitchCheatsMenu = "Switch Cheats";

        public int callbackOrder => 0;

        [MenuItem(MenuPath + SwitchCheatsMenu)]
        private static void ToggleCheats()
        {
            bool isDefineSet = IsDefineSet(Consts.Preprocessors.ENABLE_CHEATS);
            ToggleDefine(Consts.Preprocessors.ENABLE_CHEATS, !isDefineSet);
        }

        [MenuItem(MenuPath + SwitchCheatsMenu, true)]
        private static bool ToggleCheatsCheck()
        {
            bool isDefineSet = IsDefineSet(Consts.Preprocessors.ENABLE_CHEATS);
            Menu.SetChecked($"{MenuPath}{SwitchCheatsMenu}", isDefineSet);
            return true;
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            string buildMessage = null;

            if (IsDefineSet(Consts.Preprocessors.ENABLE_CHEATS))
            {
                buildMessage =
                    "Cheats modu aktifken build aliniyor. Release Build alinacagi zaman " +
                    $"{MenuPath}{SwitchCheatsMenu} Toggle Cheats kismindan devre disi birakilmalidir. " +
                    "\n\nGelistirme surecinde bu mesaj gozardi edilebilir.";
            }

            if (buildMessage != null)
            {
                EditorUtility.DisplayDialog("Uyari", buildMessage, "Tamam");
            }
        }

        private static bool IsDefineSet(string define)
        {
            string currentDefines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            return currentDefines.Contains(define);
        }

        private static void ToggleDefine(string define, bool enabled)
        {
            string currentDefines =
                PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            var definesList = new List<string>(currentDefines.Split(';'));

            if (enabled && !definesList.Contains(define))
            {
                definesList.Add(define);
            }
            else if (!enabled && definesList.Contains(define))
            {
                definesList.Remove(define);
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", definesList));
        }

        private static bool IsTouchInterface =>
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;
    }
}