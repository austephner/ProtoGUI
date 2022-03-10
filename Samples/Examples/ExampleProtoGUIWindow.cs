using UnityEngine;

namespace ProtoGUI.Examples
{
    public class ExampleProtoGUIWindow : ProtoGUIWindow
    {
        private float _testFloat;

        private ExampleEnum _testExampleEnum = ExampleEnum.EnumValueA;

        private Vector2 _scrollArea;

        protected override void DrawToolbar()
        {
            base.DrawToolbar();

            if (GUILayout.Button("Custom Menu Action"))
            {
                Debug.Log("This would've opened something had you implemented it!");
            }
        }

        protected override void DrawContent()
        {
            _scrollArea = GUILayout.BeginScrollView(_scrollArea, GUIStyle.none, GUI.skin.verticalScrollbar); 
            
            _testFloat = ProtoGUILayout.DrawHorizontalSliderField("Normal Slider", _testFloat, -1.0f, 1.0f);
            _testFloat = ProtoGUILayout.DrawFancyHorizontalSlider("Fancy Slider", _testFloat, -1.0f, 1.0f);
            _testExampleEnum = ProtoGUILayout.DrawHorizontalEnumSelector("Enum Selector", _testExampleEnum);
            _testExampleEnum = ProtoGUILayout.DrawHorizontalEnumSelectionGrid("Enum Grid", _testExampleEnum);

            GUILayout.Space(30);
            
            ProtoGUILayout.DrawVerticalMenuOptions(
                new ProtoMenuButton()
                {
                    text = "Button A"
                },
                new ProtoMenuButton()
                {
                    text = "Button B (Red)",
                    color = Color.red
                },
                new ProtoMenuButton()
                {
                    text = "Button C (Disable, Green)",
                    color = Color.green,
                    enabled = false
                });

            GUILayout.Space(15); 
            
            ProtoGUILayout.DrawInfoBox(() =>
            {
                GUILayout.Label("This is an important information update! You can use these in your prototyping GUI to call out important facts or pieces of information that are necessary to the user/player. Put whatever type of content you like inside!");
                
                if (GUILayout.Button("Even Buttons!")) { }
            });
            
            ProtoGUILayout.DrawWarningBox(() =>
            {
                GUILayout.Label("Warning! You haven't starred this repository yet! Better hop on that!");
            });
            
            ProtoGUILayout.DrawErrorBox(() =>
            {
                GUILayout.Label("There was (no) problem with drawing this box... but you could easily put an error here to signify there might've been something wrong with the project!");
            });
            
            GUILayout.EndScrollView();
        }
    }

    public enum ExampleEnum
    {
        EnumValueA,
        EnumValueB,
        EnumValueC,
        EnumValueD,
    }
}