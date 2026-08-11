using UnityEngine;

namespace SGLib.Utility.Math
{
    public readonly struct FloatRange
    {
        public readonly float Min, Max;
        public readonly float Length => Max - Min;

        public FloatRange(float min, float max)
        {
            if (min > max)
            {
                Min = max;
                Max = min;
            }
            else
            {
                Min = min;
                Max = max;
            }
        }

        public readonly float Center() => (Max + Min) * 0.5f;
        public readonly float GetRandomValue() => Random.Range(Min, Max);
    }
}