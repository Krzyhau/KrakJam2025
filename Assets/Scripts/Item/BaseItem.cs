using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	public abstract class BaseItem : MonoBehaviour
	{
		[SerializeField]
		private VortexObject _vortexObject;

		[SerializeField]
		private AnimationCurve _itemRouteCurve;

		[SerializeField]
		private AudioSource _audioSource;

		[SerializeField]
		private AudioClip thrownSound;

		[SerializeField]
		private float _orbitCatapultStrength = 8;

		private float _currentTime;
		private Rigidbody2D _rigidbody2D;
		private Collider2D _collider2D;

		protected BubbleTrigger _bubbleTrigger;

		public Rigidbody2D Rigidbody2D => _rigidbody2D != null
			? _rigidbody2D
			: _rigidbody2D = GetComponent<Rigidbody2D>();

		public Collider2D Collider2D => _collider2D != null
			? _collider2D
			: _collider2D = GetComponent<Collider2D>();

		public float CatapultStrength => _orbitCatapultStrength;

		protected virtual void OnBubbleCollided() { }

		protected virtual void OnThrowEnded() { }

		public void RegisterToVortex()
		{
			_vortexObject.Register();
		}

		public void ThrowItem(Vector3 destination, float time)
		{
			Timing.RunCoroutine(GetThrownRoutine(destination, time));
		}

		private IEnumerator<float> GetThrownRoutine(Vector3 destination, float time)
		{
			_currentTime = 0f;
			Vector3 startPosition = transform.position;

			while (_currentTime < time)
			{
				_currentTime += Time.deltaTime;
				var progress = _currentTime / time;
				transform.position = Vector3.Lerp(startPosition, destination, _itemRouteCurve.Evaluate(progress));
				yield return Timing.WaitForOneFrame;
			}

			transform.position = destination;
			OnThrowEnded();
			_audioSource.PlayOneShot(thrownSound);
		}

		private bool HasCollidedWithBubble(Collider2D collider)
		{
			return collider.gameObject.TryGetComponent(out _bubbleTrigger) && _bubbleTrigger.BubbleContext != null;
		}

		private void OnTriggerEnter2D(Collider2D collider)
		{
			if (HasCollidedWithBubble(collider))
			{
				OnBubbleCollided();
			}
		}
	}
}
