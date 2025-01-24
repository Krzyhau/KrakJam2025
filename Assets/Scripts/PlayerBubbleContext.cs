using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerBubbleContext : MonoBehaviour
	{
		[SerializeField]
		private BubbleWeightSystem _bubbleWeightSystem;

		public BubbleWeightSystem BubbleWeightSystem => _bubbleWeightSystem;
	}
}
