using System;
using System.Collections.Generic;
using UnityEngine;

namespace SGLib.Utility.Physics2D
{
    public static class Collider2DExtensions
    {
        const float HALF = 0.5f;

        public static Bounds GetWorldBounds(this BoxCollider2D boxCollider)
        {
            if (boxCollider == null)
                throw new ArgumentNullException(nameof(boxCollider));

            if (boxCollider.isActiveAndEnabled) return boxCollider.bounds;

            if (boxCollider.edgeRadius != 0f)
            {
                throw new NotSupportedException(
                    $"{nameof(GetWorldBounds)} does not support " +
                    $"{nameof(BoxCollider2D)} with a non-zero edge radius.");
            }

            var halfSize = boxCollider.size * HALF;

            var localMin = boxCollider.offset - halfSize;
            var localMax = boxCollider.offset + halfSize;

            var colliderTransform = boxCollider.transform;

            var bottomLeft = colliderTransform.TransformPoint(new Vector3(localMin.x, localMin.y, 0f));
            var bottomRight = colliderTransform.TransformPoint(new Vector3(localMax.x, localMin.y, 0f));
            var topLeft = colliderTransform.TransformPoint(new Vector3(localMin.x, localMax.y, 0f));
            var topRight = colliderTransform.TransformPoint(new Vector3(localMax.x, localMax.y, 0f));

            var bounds = new Bounds(bottomLeft, Vector3.zero);

            bounds.Encapsulate(bottomRight);
            bounds.Encapsulate(topLeft);
            bounds.Encapsulate(topRight);

            return bounds;
        }

        public static Bounds GetWorldBounds(this EdgeCollider2D edgeCollider, List<Vector2> pointsBuffer)
        {
            if (edgeCollider == null)
                throw new ArgumentNullException(nameof(edgeCollider));

            if (pointsBuffer == null)
                throw new ArgumentNullException(nameof(pointsBuffer));

            if (edgeCollider.isActiveAndEnabled) return edgeCollider.bounds;

            if (edgeCollider.edgeRadius != 0f)
                throw new NotSupportedException(
                    $"{nameof(GetWorldBounds)} does not support " +
                    $"{nameof(EdgeCollider2D)} with a non-zero edge radius.");

            pointsBuffer.Clear();
            int pointCount = edgeCollider.GetPoints(pointsBuffer);

            if (pointCount == 0)
                throw new InvalidOperationException(
                    $"{nameof(EdgeCollider2D)} contains no points.");

            var edgeTransform = edgeCollider.transform;
            var offset = edgeCollider.offset;

            var firstWorldPoint = edgeTransform.TransformPoint(pointsBuffer[0] + offset);

            var bounds = new Bounds(firstWorldPoint, Vector3.zero);

            for (int i = 1; i < pointCount; i++)
            {
                var worldPoint = edgeTransform.TransformPoint(pointsBuffer[i] + offset);

                bounds.Encapsulate(worldPoint);
            }

            return bounds;
        }
    }
}