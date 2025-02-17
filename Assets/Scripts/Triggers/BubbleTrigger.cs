using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(Collider2D))]
	public class BubbleTrigger : MonoBehaviour
	{
		[SerializeField]
		private BubbleContext bubbleContext;

		public virtual BubbleType GetBubbleType()
		{
			return BubbleType.None;
		}

		public BubbleContext BubbleContext => bubbleContext;
	}

	public enum BubbleType
	{
		None,
		Mother,
		Player
	}
}
