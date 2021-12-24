using System;
using ProtoGUI.Attributes;
using UnityEditor;
using UnityEngine;

namespace ProtoGUI.Editor
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var showIfAttribute = (EnableIfAttribute)attribute;
            var originalGuiEnabledState = GUI.enabled;

            try
            {
                var field = property.serializedObject.FindProperty(showIfAttribute.fieldName);

                switch (showIfAttribute.type)
                {
                    case EnableIfAttribute.EnableIfType.Int:
                        if (field.intValue != showIfAttribute.intValue)
                        {
                            GUI.enabled = false;
                        }
                        break;
                    default: throw new Exception($"\"ShowIf\" type {showIfAttribute.type} not available yet.");
                }
            }
            catch (Exception exception)
            {
                Debug.LogError($"Failed to draw property with \"ShowIf\", reason: {exception.Message}");
            }
            
            EditorGUI.PropertyField(position, property, label);

            GUI.enabled = originalGuiEnabledState;
        }
    }
}