using UnityEngine;

namespace SGLib.Utility.Coords2D
{
    public readonly struct Interval
    {
        public readonly float Left;
        public readonly float Right;
        public readonly float Distance => Right - Left;

        public Interval(float l, float r)
        {
            Left = l;
            Right = r;
        }

        public readonly float GetMiddlePoint() => (Right + Left) * 0.5f;

        public readonly float GetRandomPoint() => Random.Range(Left, Right);
    }
}