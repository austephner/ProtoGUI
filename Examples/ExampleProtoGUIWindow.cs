using UnityEngine;

namespace ProtoGUI.Examples
{
    public class ExampleProtoGUIWindow : ProtoGUIWindow
    {
        protected override void DrawContent()
        {
            GUILayout.FlexibleSpace();

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Hello World");
                GUILayout.FlexibleSpace();
            }

            GUILayout.FlexibleSpace();
        }
    }
}