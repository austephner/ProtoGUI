using UnityEngine;

namespace ProtoGUI.Attributes
{
    public class RandomizableIntAttribute : PropertyAttribute
    {
        public int min = int.MinValue;
        public int max = int.MaxValue;

        public RandomizableIntAttribute(int max)
        {
            min = 0;
            this.max = max;
        }

        public RandomizableIntAttribute(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
}