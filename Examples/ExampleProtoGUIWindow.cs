using UnityEngine;

namespace ProtoGUI.Examples
{
    public class ExampleProtoGUIWindow : ProtoGUIWindow
    {
        private float _testFloat;

        private ExampleEnum _testExampleEnum = ExampleEnum.EnumValueA;
        
        protected override void DrawContent()
        {
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