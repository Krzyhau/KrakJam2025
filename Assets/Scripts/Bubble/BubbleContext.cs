using UnityEngine;

namespace Monke.KrakJam2025
{
	public class BubbleContext : MonoBehaviour
	{
		[SerializeField]
		private Transform mainTransform;

		[SerializeField]
		private BubbleWeightSystem bubbleWeightSystem;

		public BubbleWeightSystem WeightSystem => bubbleWeightSystem;
		public Transform Transform => mainTransform;
	}
}
