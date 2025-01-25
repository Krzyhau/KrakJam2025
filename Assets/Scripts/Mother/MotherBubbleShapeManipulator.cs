using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class MotherBubbleShapeManipulator : MonoBehaviour
    {
        private const int LevelOfDetail = 64;

        [SerializeField] private Transform bubbleShapeOwner;
        [SerializeField] private float easingAcceleration = 5.0f;
        [SerializeField] private float easingDampening = 5.0f;
        [SerializeField] private StretchForceParameters insideStretch;
        [SerializeField] private StretchForceParameters outsideStretch;
        [SerializeField] private MeshFilter meshToUse;
        [SerializeField] private MeshData[] meshesData;

        readonly private List<MeshFilter> displayMeshes = new();
        readonly private List<Transform> stretchers = new();
        private float targetSize = 1f;
        private Mesh shapedMesh;

        private float[] shapeVelocities = Array.Empty<float>();
        private float[] shapeOffsets = Array.Empty<float>();
        private float[] lerpedShapeOffsets = Array.Empty<float>();

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
            CreateMeshes();
        }

        private void CreateMeshes()
        {
            shapedMesh = new Mesh();
            shapedMesh.MarkDynamic();

            shapedMesh.vertices = meshToUse.mesh.vertices;
            shapedMesh.triangles = meshToUse.mesh.triangles;
            shapedMesh.uv = meshToUse.mesh.uv;

            shapedMesh.RecalculateBounds();
            shapedMesh.RecalculateNormals();

            foreach (var mesh in meshesData)
            {
                var meshFilter = new GameObject("BubelMesh").AddComponent<MeshFilter>();
                var meshRenderer = meshFilter.gameObject.AddComponent<MeshRenderer>();
                meshRenderer.material = mesh.material;
                meshFilter.sharedMesh = shapedMesh;

                meshFilter.transform.localScale = new Vector3(1, mesh.Height, 1);
                meshFilter.transform.SetParent(transform, false);

                displayMeshes.Add(meshFilter);
            }
            meshToUse.gameObject.SetActive(false);
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
            RegenerateMeshes();
            EnsureProperGameObjectSize();
        }

        private Vector3 GetStretcherLocalPoint(Transform stretcher)
        {
            Vector3 stretcherLocalPoint = transform.InverseTransformPoint(stretcher.position);
            stretcherLocalPoint.y = 0.0f;
            return stretcherLocalPoint;
        }

        private void EnsureCorrectVerticesCount()
        {
            if (shapeOffsets.Length != LevelOfDetail)
            {
                shapeOffsets = new float[LevelOfDetail];
            }
        }

        private void RegenerateShapeVertices()
        {
            for (int i = 0; i < shapeOffsets.Length; i++)
            {
                float circlePosRad = i / (float)shapeOffsets.Length * Mathf.PI * 2.0f;
                Vector3 circleNormal = new(-Mathf.Cos(circlePosRad), 0, Mathf.Sin(circlePosRad));
                float stretchForce = GetStretchForceForPoint(circleNormal * targetSize);
                shapeOffsets[i] = targetSize + stretchForce;
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
            for (int i = 0; i < shapeOffsets.Length; i++)
            {
                totalDistances += shapeOffsets[i];
            }

            float averageDistance = totalDistances / shapeOffsets.Length;
            float sizeDifference = targetSize / averageDistance;

            for (int i = 0; i < shapeOffsets.Length; i++)
            {
                shapeOffsets[i] *= sizeDifference;
            }
        }

        private void RegenerateLerpedShapeVertices()
        {
            if (lerpedShapeOffsets.Length != LevelOfDetail)
            {
                lerpedShapeOffsets = new float[LevelOfDetail];
                Array.Copy(shapeOffsets, lerpedShapeOffsets, LevelOfDetail);
            }

            if (shapeVelocities.Length != LevelOfDetail)
            {
                shapeVelocities = new float[LevelOfDetail];
            }

            for (int i = 0; i < shapeOffsets.Length; i++)
            {
                shapeVelocities[i] += (shapeOffsets[i] - lerpedShapeOffsets[i]) * easingAcceleration * Time.deltaTime;
                shapeVelocities[i] *= Mathf.Max(0.0f, 1.0f - easingDampening * Time.deltaTime);
                lerpedShapeOffsets[i] += shapeVelocities[i] * Time.deltaTime;
            }
        }

        private void RegenerateMeshes()
        {
            var meshFilterVertices = shapedMesh.vertices;

            for (int i = 0; i < meshFilterVertices.Length; i++)
            {
                var vertex = meshFilterVertices[i];
                float horizontalAngle = Mathf.Atan2(-vertex.z, vertex.x);
                int vertexIndex = Mathf.RoundToInt((horizontalAngle / (Mathf.PI * 2.0f) + 0.5f) * LevelOfDetail) % LevelOfDetail;
                meshFilterVertices[i] = meshToUse.sharedMesh.vertices[i] * lerpedShapeOffsets[vertexIndex] / targetSize;
                meshFilterVertices[i].y = meshToUse.sharedMesh.vertices[i].y;
            }

            shapedMesh.vertices = meshFilterVertices;
        }

        private void EnsureProperGameObjectSize()
        {
            float currentSize = bubbleShapeOwner.localScale.x;
            if (currentSize == targetSize)
            {
                return;
            }

            bubbleShapeOwner.localScale = new Vector3(targetSize, targetSize, targetSize);

            for(int i = 0; i < lerpedShapeOffsets.Length; i++)
            {
                lerpedShapeOffsets[i] *= currentSize / targetSize;
            }
        }

        [Serializable]
        public struct StretchForceParameters
        {
            public float Distance;
            public float Exponent;
            public float Force;
        }

        [Serializable]
        public struct MeshData
        {
            public float Height;
            public Material material;
        }
    }
}
