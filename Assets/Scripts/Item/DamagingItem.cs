using System;

namespace Monke.KrakJam2025
{
	public class DamagingItem : BaseItem
	{
		protected override void OnBubbleCollided()
		{
			if (_bubbleTrigger is PlayerTrigger playerTrigger)
			{
				playerTrigger.PlayerBubbleContext.PlayerDeath.Death();
			}

			if (_bubbleTrigger is MotherTrigger motherTrigger)
			{
				motherTrigger.TriggerDeath();
			}
		}
	}
}