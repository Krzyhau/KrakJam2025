using System;

namespace Monke.KrakJam2025
{
	public class DamagingItem : BaseItem
	{
		public event Action<BubbleContext> OnSharpTouchedBubble;

		protected override void OnBubbleCollided()
		{
			OnSharpTouchedBubble?.Invoke(_bubbleTrigger.BubbleContext);
		}
	}
}