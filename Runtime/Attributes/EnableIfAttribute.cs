using UnityEngine;

namespace ProtoGUI.Attributes
{
    /// <summary>
    /// Enables an editor inspector field only when the given class's field matches the given value.
    /// </summary>
    public class EnableIfAttribute : PropertyAttribute
    {
        public string fieldName;

        public int intValue;

        public EnableIfType type;

        public EnableIfAttribute(string fieldName, int intValue)
        {
            this.fieldName = fieldName;
            this.intValue = intValue;
            type = EnableIfType.Int;
        }

        public enum EnableIfType
        {
            String,
            Int,
            Bool
        }
    }
}