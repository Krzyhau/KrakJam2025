using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerBubbleContext : MonoBehaviour
	{
		[SerializeField]
		private BubbleWeightSystem _bubbleWeightSystem;

		[SerializeField]
		private PlayerController _playerController;

		public Transform Transform => _playerController.transform;

		public BubbleWeightSystem WeightSystem => _bubbleWeightSystem;

		public PlayerController Input => _playerController;
	}
}
