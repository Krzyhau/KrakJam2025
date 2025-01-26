using Rubin;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class MotherTrigger : BubbleTrigger
	{
		[SerializeField]
		private MotherPlayerController mother;

		[SerializeField]
		private MotherDeathHandler motherDeathHandler;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private float bounceForce;

		[SerializeField]
		private float cooldown = 2;

		private Ticker ticker;

		public void TriggerDeath()
		{
			motherDeathHandler.TriggerDeath();
		}

		public override BubbleType GetBubbleType()
		{
			return BubbleType.Mother;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (IsReady(collision, out var playerTrigger))
			{
				mother.AddPlayerInside(playerTrigger.PlayerBubbleContext);
				ticker = TickerCreator.CreateNormalTime(cooldown);
				ticker.Reset();
			}

			if (collision.gameObject.CompareTag("Bouncable"))
			{
				var bounceDirection = this.transform.position - collision.gameObject.transform.position;
				rb2d.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
				// Possible audio sound?
			}
		}

		private bool IsReady(Collider2D collider, out PlayerTrigger playerTrigger)
		{
			playerTrigger = null;
			return collider.TryGetComponent(out playerTrigger) && playerTrigger.PlayerBubbleContext != null;
		}
	}
}
