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

		private void OnItemCollected(BubbleContext bubbleContext)
		{
			if (weightValue > 0)
			{
                bubbleContext.WeightSystem.AddWeight(weightValue);
            }
			else if (weightValue < 0) 
			{
				bubbleContext.WeightSystem.RemoveWeight(Mathf.Abs(weightValue));
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
