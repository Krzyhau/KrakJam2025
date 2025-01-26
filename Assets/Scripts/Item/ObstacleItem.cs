using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ObstacleItem : BaseItem
	{
		[SerializeField]
		private float _bounceForce = 4f;

		protected override void OnBubbleCollided()
		{
			if (_bubbleTrigger.GetBubbleType() == BubbleType.Player)
			{
				var bounceDirection = transform.position - _bubbleTrigger.BubbleContext.Transform.position;
				Rigidbody2D.AddForce(bounceDirection * _bounceForce, ForceMode2D.Impulse);
			}
		}
	}
}
