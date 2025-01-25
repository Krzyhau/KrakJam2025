using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerBubbleContext : BubbleContext
	{
		[SerializeField]
		private PlayerController _playerController;

		[SerializeField]
		private Collider2D _collider2d;

		public PlayerController PlayerController => _playerController;

		public Collider2D Collider2D => _collider2d;
	}
}
