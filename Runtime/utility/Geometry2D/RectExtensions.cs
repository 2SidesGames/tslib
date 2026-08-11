using UnityEngine;

namespace SGLib.Utility.Geometry2D
{
    public static class RectExtensions
    {
        public static bool IsApproximatelyEqualTo(this Rect rect, Rect other, float tolerance = 0.5f)
        {
            return
                Mathf.Abs(rect.xMin - other.xMin) <= tolerance &&
                Mathf.Abs(rect.yMin - other.yMin) <= tolerance &&
                Mathf.Abs(rect.xMax - other.xMax) <= tolerance &&
                Mathf.Abs(rect.yMax - other.yMax) <= tolerance;
        }
    }
}