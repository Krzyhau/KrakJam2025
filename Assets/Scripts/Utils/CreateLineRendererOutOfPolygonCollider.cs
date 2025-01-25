using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(PolygonCollider2D))]
	[ExecuteInEditMode]
	public class CreateLineRendererOutOfPolygonCollider : MonoBehaviour
	{
		private void Start()
		{
			var lineRenderer = gameObject.AddComponent<LineRenderer>();
			lineRenderer.positionCount = GetComponent<PolygonCollider2D>().points.Length;
			lineRenderer.useWorldSpace = false;
			lineRenderer.loop = true;
			lineRenderer.startWidth = 0.1f;
			lineRenderer.endWidth = 0.1f;
			lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
			lineRenderer.startColor = Color.red;
			lineRenderer.endColor = Color.red;
			lineRenderer.sortingLayerName = "Foreground";
			lineRenderer.sortingOrder = 1;
			for (var i = 0; i < GetComponent<PolygonCollider2D>().points.Length; i++)
			{
				lineRenderer.SetPosition(i, GetComponent<PolygonCollider2D>().points[i]);
			}
		}
	}
}