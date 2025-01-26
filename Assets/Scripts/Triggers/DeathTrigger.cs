using UnityEngine;

namespace Monke.KrakJam2025
{
	public class DeathTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (IsMother(collision, out MotherTrigger motherTrigger))
			{
				motherTrigger.TriggerDeath();
			}

			if (IsPlayer(collision, out PlayerTrigger playerTrigger))
			{
				playerTrigger.PlayerBubbleContext.PlayerDeath.Death();
			}

			if (IsItem(collision, out BaseItem item))
			{
				Destroy(item.gameObject);
			}
		}

		private bool IsPlayer(Collider2D collider, out PlayerTrigger playerTrigger)
		{
			playerTrigger = null;
			return collider.TryGetComponent(out playerTrigger) && playerTrigger.PlayerBubbleContext != null;
		}

		private bool IsItem(Collider2D collider, out BaseItem item)
		{
			item = null;
			return collider.TryGetComponent(out item);
		}

		private bool IsMother(Collider2D collider, out MotherTrigger motherTrigger)
		{
			motherTrigger = null;
			return collider.TryGetComponent(out motherTrigger);
		}
	}
}
