using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerBubbleContext : MonoBehaviour
	{
		[SerializeField]
		private BubbleWeightSystem _bubbleWeightSystem;

		[SerializeField]
		private PlayerController _playerController;

		public BubbleWeightSystem BubbleWeightSystem => _bubbleWeightSystem;

		public PlayerController PlayerController => _playerController;
	}
}
