using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Monke.KrakJam2025
{
    public class MotherBubbleShapeManipulator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SpriteShapeController spriteShapeController;

        [Header("Properties")]
        [Min(4), SerializeField] private int levelOfDetail;
        [SerializeField] private float lerpSpeed = 5.0f;
        [SerializeField] private StretchForceParameters insideStretch;
        [SerializeField] private StretchForceParameters outsideStretch;

        [SerializeField] private List<Transform> stretchers = new();

        private float targetSize;
        private Vector3[] shapeVertices = Array.Empty<Vector3>();
        private Vector3[] lerpedShapeVertices = Array.Empty<Vector3>();

        public void SetTargetSize(float size)
        {
            targetSize = size;
        }

        public void SetStretcherState(Transform transform, bool state)
        {
            if (state && !stretchers.Contains(transform))
            {
                stretchers.Add(transform);
            }
            else if (!state && stretchers.Contains(transform))
            {
                stretchers.Remove(transform);
            }
        }

        private void Start()
        {
            SetTargetSize(4f);
        }

        private void Update()
        {
            RegenerateShape();
        }

        private void RegenerateShape()
        {
            EnsureCorrectVerticesCount();
            RegenerateShapeVertices();
            NormalizeDistancesToSize();
            RegenerateLerpedShapeVertices();

            Spline spline = spriteShapeController.spline;
            spline.Clear();

            for (int i = 0; i < lerpedShapeVertices.Length; i++)
            {
                spline.InsertPointAt(i, lerpedShapeVertices[i]);

                var prevPoint = lerpedShapeVertices[i == 0 ? lerpedShapeVertices.Length - 1 : i - 1];
                var nextPoint = lerpedShapeVertices[i == lerpedShapeVertices.Length - 1 ? 0 : i + 1];

                var tangentVector = (nextPoint - prevPoint) * 0.25f;

                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spline.SetLeftTangent(i, tangentVector * -1.0f);
                spline.SetRightTangent(i, tangentVector);
            }

            spriteShapeController.RefreshSpriteShape();
        }

        private void EnsureCorrectVerticesCount()
        {
            if (shapeVertices.Length != levelOfDetail)
            {
                shapeVertices = new Vector3[levelOfDetail];
            }
        }

        private void RegenerateShapeVertices()
        {
            for (int i = 0; i < shapeVertices.Length; i++)
            {
                float circlePosRad = i / (float)shapeVertices.Length * Mathf.PI * 2.0f;
                Vector3 circleNormal = new(Mathf.Cos(circlePosRad), Mathf.Sin(circlePosRad), 0);

                var circlePoint = circleNormal * targetSize;

                float stretchForce = GetStretchForceForPoint(circlePoint);
                circlePoint += stretchForce * circleNormal;

                shapeVertices[i] = circlePoint;
            }
        }

        private float GetStretchForceForPoint(Vector3 vertexPosition)
        {
            float stretchForce = 0.0f;

            foreach (var stretcher in stretchers)
            {
                stretchForce += GetStretcherForceInPoint(vertexPosition, stretcher);
            }

            return stretchForce;
        }

        float GetStretcherForceInPoint(Vector3 vertexPoint, Transform stretcher)
        {
            Vector3 stretcherLocalPoint = transform.InverseTransformPoint(stretcher.position);
            stretcherLocalPoint.z = 0.0f;

            Vector3 stretcherToVertexDelta = vertexPoint - stretcherLocalPoint;

            bool isInside = stretcherLocalPoint.magnitude < targetSize;
            var stretchParams = isInside ? insideStretch : outsideStretch;
            float scalar = isInside ? 1.0f : -1.0f;

            float stretchDelta = stretcherToVertexDelta.magnitude / stretchParams.Distance;
            float stretchExpDelta = Mathf.Pow(stretchDelta, stretchParams.Exponent);
            return Mathf.Lerp(stretchParams.Force, 0f, stretchExpDelta) * scalar;
        }

        private void NormalizeDistancesToSize()
        {
            float totalDistances = 0.0f;
            for (int i = 0; i < shapeVertices.Length; i++)
            {
                totalDistances += shapeVertices[i].magnitude;
            }

            float averageDistance = totalDistances / shapeVertices.Length;
            float sizeDifference = targetSize / averageDistance;

            for (int i = 0; i < shapeVertices.Length; i++)
            {
                shapeVertices[i] *= sizeDifference;
            }
        }

        private void RegenerateLerpedShapeVertices()
        {
            if (lerpedShapeVertices.Length != levelOfDetail)
            {
                lerpedShapeVertices = new Vector3[levelOfDetail];
                Array.Copy(shapeVertices, lerpedShapeVertices, levelOfDetail);
            }

            for (int i = 0; i < shapeVertices.Length; i++)
            {
                lerpedShapeVertices[i] = Vector3.Lerp(lerpedShapeVertices[i], shapeVertices[i], Time.deltaTime * lerpSpeed);
            }
        }

        [Serializable]
        public struct StretchForceParameters
        {
            public float Distance;
            public float Exponent;
            public float Force;
        }
    }
}
