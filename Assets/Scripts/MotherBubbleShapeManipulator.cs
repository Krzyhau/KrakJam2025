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
        [SerializeField] private StretchForceParameters insideStretch;
        [SerializeField] private StretchForceParameters outsideStretch;

        [SerializeField] private List<Transform> insideStretchers = new();
        [SerializeField] private List<Transform> outsideStretchers = new();

        private float targetSize;
        private Vector3[] shapeVertices = Array.Empty<Vector3>();

        public void SetTargetSize(float size)
        {
            targetSize = size;
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
            RegenerateShapeVertices();

            Spline spline = spriteShapeController.spline;
            spline.Clear();

            for (int i = 0; i < shapeVertices.Length; i++)
            {
                spline.InsertPointAt(i, shapeVertices[i]);

                var prevPoint = shapeVertices[i == 0 ? shapeVertices.Length - 1 : i - 1];
                var nextPoint = shapeVertices[i == shapeVertices.Length - 1 ? 0 : i + 1];

                var tangentVector = (nextPoint - prevPoint) * 0.25f;

                spline.SetTangentMode(i, ShapeTangentMode.Continuous);
                spline.SetLeftTangent(i, tangentVector * -1.0f);
                spline.SetRightTangent(i, tangentVector);
            }

            spriteShapeController.RefreshSpriteShape();
        }

        private void RegenerateShapeVertices()
        {
            int vertices = levelOfDetail + 4;

            if(shapeVertices.Length != vertices)
            {
                shapeVertices = new Vector3[vertices];
            }

            for (int i = 0; i < vertices; i++)
            {
                float circlePosRad = i / (float)vertices * Mathf.PI * 2.0f;
                Vector3 circleNormal = new(Mathf.Cos(circlePosRad), Mathf.Sin(circlePosRad), 0);

                var circlePoint = circleNormal * targetSize;

                float stretchForce = GetStretchForceForPoint(circlePoint);
                circlePoint += stretchForce * circleNormal;

                shapeVertices[i] = circlePoint;
            }

            NormalizeDistancesToSize();
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

        private float GetStretchForceForPoint(Vector3 vertexPosition)
        {
            float stretchForce = 0.0f;
            Vector3 vertexWorldPoint = transform.TransformPoint(vertexPosition);

            foreach (var insideStretcher in insideStretchers)
            {
                stretchForce += GetStretcherForceInPoint(vertexWorldPoint, insideStretcher, insideStretch);
            }

            foreach (var outsideStretcher in outsideStretchers)
            {
                stretchForce -= GetStretcherForceInPoint(vertexWorldPoint, outsideStretcher, outsideStretch);
            }

            return stretchForce;
        }

        float GetStretcherForceInPoint(Vector3 vertexWorldPoint, Transform stretcher, StretchForceParameters stretchParams)
        {
            Vector3 deltaVector = stretcher.position - vertexWorldPoint;
            Vector3 projectedDeltaVector = Vector3.ProjectOnPlane(deltaVector, transform.forward);
            float stretchDelta = deltaVector.magnitude / stretchParams.Distance;
            float stretchExpDelta = Mathf.Pow(stretchDelta, stretchParams.Exponent);
            return Mathf.Lerp(stretchParams.Force, 0f, stretchExpDelta);
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
