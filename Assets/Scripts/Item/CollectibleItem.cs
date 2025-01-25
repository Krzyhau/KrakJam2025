using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class CollectibleItem : BaseItem
	{
		public event Action<BubbleContext> OnItemCollectedEvent;

		[SerializeField]
		private bool removeOnCollected = true;

		protected override void OnBubbleCollided()
		{
			OnItemCollectedEvent?.Invoke(_bubbleTrigger.BubbleContext);

			if (removeOnCollected)
			{
				Destroy(gameObject);
			}
		}
	}
}
