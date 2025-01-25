using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerBubbleContext : BubbleContext
	{
		[SerializeField]
		private PlayerController _playerController;

		public PlayerController PlayerController => _playerController;
	}
}
