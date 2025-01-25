using UnityEngine;

namespace Monke.KrakJam2025
{
	public class PlayerTrigger : MonoBehaviour
	{
		[SerializeField]
		private PlayerBubbleContext _playerBubbleContext;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private float bounceForce;

		public PlayerBubbleContext PlayerBubbleContext => _playerBubbleContext;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.tag == "Bouncable")
			{
				var bounceDirection = this.transform.position - collision.gameObject.transform.position;
				rb2d.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
			}
		}
	}
}
