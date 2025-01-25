using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(Collider2D))]
	public class CollectibleItem : MonoBehaviour
	{
		public event Action<BubbleContext> OnItemCollectedEvent;

		[SerializeField]
		private bool removeOnCollected = true;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.TryGetComponent(out BubbleTrigger bubbleTrigger) && bubbleTrigger.BubbleContext != null)
			{
				OnItemCollectedEvent?.Invoke(bubbleTrigger.BubbleContext);
			}
			
			if (removeOnCollected)
			{
				Destroy(gameObject);
			}
		}
	}
}
