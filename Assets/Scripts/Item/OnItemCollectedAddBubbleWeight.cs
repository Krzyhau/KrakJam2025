using UnityEngine;

namespace Monke.KrakJam2025
{
	public class OnItemCollectedAddBubbleWeight : MonoBehaviour
	{
		[SerializeField]
		private CollectibleItem collectibleItem;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip absorbSound;

		[SerializeField]
		private AudioClip damageSound;

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
				audioSource.PlayOneShot(damageSound);
			}

			collectibleItem.OnItemCollectedEvent -= OnItemCollected;
			audioSource.PlayOneShot(absorbSound);
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
