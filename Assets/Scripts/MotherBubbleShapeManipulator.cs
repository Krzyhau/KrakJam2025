using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

namespace Monke.KrakJam2025
{
    [ExecuteAlways]
    public class MotherBubbleShapeManipulator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SpriteShapeController spriteShapeController;

        [Header("Properties")]
        [Min(4), SerializeField] private int levelOfDetail;
        [SerializeField] private float easingSpeed = 5.0f;
        [SerializeField] private float stretchersLerpSpeed = 5.0f;
        [SerializeField] private StretchForceParameters insideStretch;
        [SerializeField] private StretchForceParameters outsideStretch;

        [SerializeField] private List<Transform> stretchers = new();

        private float targetSize;
        private readonly Dictionary<Transform, float> stretchersLerpStates = new();
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
                stretchersLerpStates.Add(transform, 0.0f);
            }
            else if (!state && stretchers.Contains(transform))
            {
                stretchers.Remove(transform);
                stretchersLerpStates.Remove(transform);
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
            UpdateStretchersLerpValues();
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

        private void UpdateStretchersLerpValues()
        {
            foreach(var stretcher in stretchers) {
                if(!stretchersLerpStates.ContainsKey(stretcher))
                {
                    stretchersLerpStates[stretcher] = 0.0f;
                }

                var stretcherLocalPoint = GetStretcherLocalPoint(stretcher);
                float distance = stretcherLocalPoint.magnitude;
                float targetLerp = distance < targetSize ? 1.0f : 0.0f;
                stretchersLerpStates[stretcher] = Mathf.MoveTowards(
                    stretchersLerpStates[stretcher], targetLerp, stretchersLerpSpeed * Time.deltaTime);
            }
        }

        private Vector3 GetStretcherLocalPoint(Transform stretcher)
        {
            Vector3 stretcherLocalPoint = transform.InverseTransformPoint(stretcher.position);
            stretcherLocalPoint.z = 0.0f;
            return stretcherLocalPoint;
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
            Vector3 stretcherLocalPoint = GetStretcherLocalPoint(stretcher);
            Vector3 stretcherToVertexDelta = vertexPoint - stretcherLocalPoint;

            float stretcherLerpState = StretcherLerpStateToScalar(stretcher);
            bool isInside = stretcherLerpState >= 0.5f;
            var stretchParams = isInside ? insideStretch : outsideStretch;

            float stretchDelta = stretcherToVertexDelta.magnitude / stretchParams.Distance;
            float stretchExpDelta = Mathf.Pow(stretchDelta, stretchParams.Exponent);
            return Mathf.Lerp(stretchParams.Force, 0f, stretchExpDelta) * stretcherLerpState;
        }

        float StretcherLerpStateToScalar(Transform stretcher)
        {
            float stretcherLerpState = EaseInOutQuint(stretchersLerpStates[stretcher]);
            return Mathf.Lerp(-1.0f, 1.0f, stretcherLerpState);

            float EaseInOutQuint(float x)
            {
                return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
            }
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
                lerpedShapeVertices[i] = Vector3.Lerp(lerpedShapeVertices[i], shapeVertices[i], Time.deltaTime * easingSpeed);
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
