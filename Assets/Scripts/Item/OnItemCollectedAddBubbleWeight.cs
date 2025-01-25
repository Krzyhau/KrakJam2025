using UnityEngine;

namespace Monke.KrakJam2025
{
	public class OnItemCollectedAddBubbleWeight : MonoBehaviour
	{
		[SerializeField]
		private CollectibleItem collectibleItem;

		[SerializeField]
		private float weightValue;

		private void Start()
		{
			collectibleItem.OnItemCollectedEvent += OnItemCollected;
		}

		private void OnItemCollected(PlayerBubbleContext playerBubbleContext)
		{
			playerBubbleContext.WeightSystem.AddWeight(weightValue);
			collectibleItem.OnItemCollectedEvent -= OnItemCollected;
		}

		private void OnDestroy()
		{
			if (collectibleItem != null)
			{
				collectibleItem.OnItemCollectedEvent -= OnItemCollected;
			}
		}
	}
}
