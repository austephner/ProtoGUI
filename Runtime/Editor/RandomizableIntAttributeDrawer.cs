#if UNITY_EDITOR
using ProtoGUI.Attributes;
using UnityEditor;
using UnityEngine;

namespace ProtoGUI.Editor
{
    [CustomPropertyDrawer(typeof(RandomizableIntAttribute))]
    public class RandomizableIntAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.width -= 30;

            EditorGUI.PropertyField(position, property, label);

            var randomizableAttribute = (RandomizableIntAttribute)attribute;

            position.x += position.width;
            position.width = 30;

            if (GUI.Button(position, new GUIContent("R", "Randomize")))
            {
                property.intValue = Random.Range(randomizableAttribute.min, randomizableAttribute.max);
            }
        }
    }
}
#endif