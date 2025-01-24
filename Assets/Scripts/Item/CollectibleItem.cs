using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(Collider2D))]
	public class CollectibleItem : MonoBehaviour
	{
		public event Action<PlayerBubbleContext> OnItemCollectedEvent;

		[SerializeField]
		private bool removeOnCollected = true;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.TryGetComponent(out PlayerTrigger playerTrigger) && playerTrigger.PlayerBubbleContext != null)
			{
				OnItemCollectedEvent?.Invoke(playerTrigger.PlayerBubbleContext);
			}
			
			if (removeOnCollected)
			{
				Destroy(gameObject);
			}
		}
	}
}
