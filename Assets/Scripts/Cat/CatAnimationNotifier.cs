using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class CatAnimationNotifier : MonoBehaviour
	{
		public event Action OnThrowItemEvent;
		public event Action OnThrowEndedEvent;

		[SerializeField]
		private Animator _catAnimator;

		private readonly int ThrowHash = Animator.StringToHash("Throw");

		public void InvokeThrowAnimation()
		{
			_catAnimator.SetTrigger(ThrowHash);
		}

		// Invoked by animator
		private void OnThrowItem()
		{
			OnThrowItemEvent?.Invoke();
		}

		// Invoked by animator
		private void OnThrowEnded()
		{
			OnThrowEndedEvent?.Invoke();
		}
	}
}
