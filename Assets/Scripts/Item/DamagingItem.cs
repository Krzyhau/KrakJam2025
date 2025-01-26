using UnityEngine;

namespace Monke.KrakJam2025
{
	public class DamagingItem : BaseItem
	{
		[SerializeField]
		private float _damage = 40;

		protected override void OnBubbleCollided()
		{
			if (_bubbleTrigger is PlayerTrigger playerTrigger)
			{
				playerTrigger.PlayerBubbleContext.PlayerDeath.Death();
			}

			if (_bubbleTrigger is MotherTrigger motherTrigger)
			{
				motherTrigger.BubbleContext.WeightSystem.AddWeight(-_damage);
				Destroy(gameObject);

				if (motherTrigger.BubbleContext.WeightSystem.Weight < 0)
				{
					motherTrigger.TriggerDeath();
				}
			}
		}
	}
}