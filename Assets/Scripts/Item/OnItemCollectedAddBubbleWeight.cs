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
			if (weightValue > 0)
			{
                playerBubbleContext.WeightSystem.AddWeight(weightValue);
            }
			else if (weightValue < 0) 
			{
				playerBubbleContext.WeightSystem.RemoveWeight(Mathf.Abs(weightValue));
			}

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
