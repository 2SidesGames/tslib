using UnityEngine;

namespace SGLib.Utility.Geometry2D
{
    public static class Vector2Extensions
    {
        public static float SqrDistanceToSegment(this Vector2 point, Vector2 start, Vector2 end)
        {
            var segment = end - start;
            float segmentLengthSquared = segment.sqrMagnitude;

            if (segmentLengthSquared <= Mathf.Epsilon)
                return (point - start).sqrMagnitude;

            float t = Vector2.Dot(point - start, segment) / segmentLengthSquared;
            var closestPoint = start + segment * Mathf.Clamp01(t);

            return (point - closestPoint).sqrMagnitude;
        }
    }
}