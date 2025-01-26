using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class DeathTrigger : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (IsPlayer(collision, out var playerTrigger))
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
			var state = collider.gameObject.TryGetComponent(out playerTrigger) && playerTrigger.PlayerBubbleContext != null;
			return state;
		}

		private bool IsItem(Collider2D collider, out BaseItem item)
		{
			item = null;
			return collider.gameObject.TryGetComponent(out item);
		}
	}
}
