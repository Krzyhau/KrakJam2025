using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
    [ExecuteAlways]
    public class MotherBubbleShapeManipulator : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private MeshFilter meshFilter;

        [Header("Properties")]
        [Min(4), SerializeField] private int levelOfDetail;
        [SerializeField] private float easingAcceleration = 5.0f;
        [SerializeField] private float easingDampening = 5.0f;
        [SerializeField] private StretchForceParameters insideStretch;
        [SerializeField] private StretchForceParameters outsideStretch;

        [SerializeField] private List<Transform> stretchers = new();

        [SerializeField]
        private float targetSize;
        private Vector3[] shapeVertexVelocities = Array.Empty<Vector3>();
        private Vector3[] shapeVertices = Array.Empty<Vector3>();
        private Vector3[] lerpedShapeVertices = Array.Empty<Vector3>();
        private Mesh mesh;

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
            RegenerateMesh();
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

            var vertices = new Vector3[shapeVertices.Length + 1];
            vertices[0] = Vector3.zero;
            for (int i = 0; i < shapeVertices.Length; i++)
            {
                vertices[i+1] = lerpedShapeVertices[i];
            }
            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
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
                RegenerateMesh();
            }
        }

        private void RegenerateMesh()
        {
            if(mesh != null)
            {
#if !UNITY_EDITOR
                Destroy(mesh);
#else
                DestroyImmediate(mesh);
#endif
            }

            mesh = new Mesh();
            mesh.name = "MotherBubbleMesh";
            mesh.MarkDynamic();

            var vertices = new Vector3[shapeVertices.Length + 1];
            var uvs = new Vector2[shapeVertices.Length + 1];
            var triangles = new int[shapeVertices.Length * 3];

            uvs[0] = new Vector2(0.5f, 0.5f);
            vertices[0] = Vector3.zero;

            for (int i = 0; i < shapeVertices.Length; i++)
            {
                float circlePosRad = i / (float)shapeVertices.Length * Mathf.PI * 2.0f;
                Vector2 circleNormal = new(Mathf.Cos(circlePosRad), Mathf.Sin(circlePosRad));
                uvs[i+1] = (Vector2.one + circleNormal) * 0.5f;

                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = (i + 1) % shapeVertices.Length + 1;
            }

            mesh.vertices = vertices;
            mesh.uv = uvs;
            mesh.triangles = triangles;

            meshFilter.mesh = mesh;
        }

        private void RegenerateShapeVertices()
        {
            for (int i = 0; i < shapeVertices.Length; i++)
            {
                float circlePosRad = i / (float)shapeVertices.Length * Mathf.PI * 2.0f;
                Vector3 circleNormal = new(-Mathf.Cos(circlePosRad), Mathf.Sin(circlePosRad), 0);

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

            bool isInside = stretcherLocalPoint.magnitude <= targetSize;
            var stretchParams = isInside ? insideStretch : outsideStretch;
            var stretchMultiplier = isInside ? 1.0f : -1.0f;

            float stretchDelta = stretcherToVertexDelta.magnitude / stretchParams.Distance;
            float stretchExpDelta = Mathf.Pow(stretchDelta, stretchParams.Exponent);
            return Mathf.Lerp(stretchParams.Force, 0f, stretchExpDelta) * stretchMultiplier;
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

            if (shapeVertexVelocities.Length != levelOfDetail)
            {
                shapeVertexVelocities = new Vector3[levelOfDetail];
            }

            for (int i = 0; i < shapeVertices.Length; i++)
            {
                shapeVertexVelocities[i] += (shapeVertices[i] - lerpedShapeVertices[i]) * easingAcceleration * Time.deltaTime;
                shapeVertexVelocities[i] *= Mathf.Max(0.0f, 1.0f - easingDampening * Time.deltaTime);
                lerpedShapeVertices[i] += shapeVertexVelocities[i] * Time.deltaTime;
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
