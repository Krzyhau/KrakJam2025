using UnityEngine;

namespace Monke.KrakJam2025
{
	public class BubbleContext : MonoBehaviour
	{
		[SerializeField]
		private Transform mainTransform;

		[SerializeField]
		private BubbleWeightSystem bubbleWeightSystem;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private AudioClip returnSound;

		[SerializeField]
		private AudioClip splitSound;

		public BubbleWeightSystem WeightSystem => bubbleWeightSystem;
		public Transform Transform => mainTransform;
		public AudioSource AudioSource => audioSource;
		public AudioClip AbsorbSound => returnSound;
		public AudioClip SplitSound => splitSound;
	}
}
