using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(Collider2D))]
	public class BubbleTrigger : MonoBehaviour
	{
		[SerializeField]
		private BubbleContext bubbleContext;

		public BubbleContext BubbleContext => bubbleContext;
	}
}
