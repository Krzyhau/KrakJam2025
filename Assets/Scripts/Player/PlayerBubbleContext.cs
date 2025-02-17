using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerBubbleContext : BubbleContext
	{
		[SerializeField]
		private PlayerController _playerController;

		[SerializeField]
		private Collider2D _collider2d;

		[SerializeField]
		private PlayerDeathHandler _playerDeath;

		public PlayerController PlayerController => _playerController;

		public Collider2D Collider2D => _collider2d;

		public PlayerDeathHandler PlayerDeath => _playerDeath;
	}
}
