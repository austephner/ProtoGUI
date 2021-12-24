using UnityEditor;
using UnityEngine;

namespace ProtoGUI.Editor
{
    [CustomEditor(typeof(ProtoGUIWindow), true)]
    public class ProtoGUIWindowDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Space(15);
            GUILayout.Label("Runtime Only", EditorStyles.boldLabel);

            var originalGuiEnabledStatus = GUI.enabled;
            GUI.enabled = EditorApplication.isPlaying;

            var window = (ProtoGUIWindow)target;

            window.show = EditorGUILayout.ToggleLeft("Show", window.show);
            window.minimized = EditorGUILayout.ToggleLeft("Minimized", window.minimized);

            GUI.enabled = true;

            GUILayout.Space(15);
            GUILayout.Label("Staistics", EditorStyles.boldLabel);

            GUI.enabled = false;
            
            GUILayout.Label($"Rect:\t\t{window.rect.ToString()}");
            GUILayout.Label($"Last Shown:\t{window.lastShown:g}");
            GUILayout.Label($"Last Hidden:\t{window.lastHidden:g}");
            
            GUI.enabled = originalGuiEnabledStatus;
        }
    }
}