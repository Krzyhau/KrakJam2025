using UnityEngine;

namespace Monke.KrakJam2025
{
	public class BubbleWeightSystem : MonoBehaviour
	{
		[SerializeField]
		private Transform bubbleMainTransform;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private AnimationCurve weightToScaleCurve;

		[SerializeField]
		private AnimationCurve weightToMassCurve;

		[SerializeField]
		private float weight;

		public float Weight
		{
			get => weight;
			private set
			{
				weight = value;
				UpdateMassAndScale();
			}
		}

		public void AddWeight(float additionalWeight)
		{
			Weight += additionalWeight;
		}

		public void RemoveWeight(float weight)
		{
			Weight -= weight;
		}

		public void SetWeight(float newWeight)
		{
			Weight = newWeight;
		}

		private void UpdateMassAndScale()
		{
			bubbleMainTransform.localScale = Vector2.one * weightToScaleCurve.Evaluate(weight);
			rb2d.mass = weightToMassCurve.Evaluate(weight);
		}
	}
}
