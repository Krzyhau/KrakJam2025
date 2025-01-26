using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerTrigger : BubbleTrigger
	{
		[SerializeField]
		private PlayerBubbleContext _playerBubbleContext;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private float bounceForce;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip bounceSound;

		public PlayerBubbleContext PlayerBubbleContext => _playerBubbleContext;

		public override BubbleType GetBubbleType()
		{
			return BubbleType.Player;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.CompareTag("Bouncable"))
			{
				var bounceDirection = this.transform.position - collision.gameObject.transform.position;
				rb2d.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
				audioSource.PlayOneShot(bounceSound);
			}
		}
	}
}
