using System;
using UnityEngine;

namespace SGLib.Utility.Geometry2D
{
    public static class Bounds2DExtensions
    {
        public static Vector2 GetNormalizedCenterIn(this Bounds content, Bounds container)
        {
            if (!container.FullyContains(content))
            {
                throw new InvalidOperationException(
                    $"{nameof(content)} is not fully contained within {nameof(container)}.");
            }

            return new Vector2(
                x: Mathf.InverseLerp(
                    container.min.x + content.extents.x,
                    container.max.x - content.extents.x,
                    content.center.x),

                y: Mathf.InverseLerp(
                    container.min.y + content.extents.y,
                    container.max.y - content.extents.y,
                    content.center.y)
            );
        }

        public static Vector2 GetWorldCenterIn(this Bounds content, Bounds container, Vector2 normalizedPosition)
        {
            if (!content.FitsWithin(container))
            {
                throw new InvalidOperationException(
                    $"{nameof(content)} does not fit within {nameof(container)}.");
            }

            return new Vector2(
                Mathf.Lerp(
                    container.min.x + content.extents.x,
                    container.max.x - content.extents.x,
                    normalizedPosition.x),

                Mathf.Lerp(
                    container.min.y + content.extents.y,
                    container.max.y - content.extents.y,
                    normalizedPosition.y)
            );
        }

        public static bool FullyContains(this Bounds container, Bounds content)
        {
            return
                content.min.x >= container.min.x &&
                content.max.x <= container.max.x &&
                content.min.y >= container.min.y &&
                content.max.y <= container.max.y;
        }

        public static bool FitsWithin(this Bounds content, Bounds container)
        {
            return content.size.x <= container.size.x && content.size.y <= container.size.y;
        }
    }
}