using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private Transform visuals;

		public event Action<PlayerBubbleContext> OnPlayerSplit;

		[SerializeField]
		private float movementSpeed = 10f;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private PlayerBubbleContext playerContext;

		private PlayerSpawnPoint spawnPoint;
		private PlayerSpawnPoint SpawnPoint => spawnPoint != null 
			? spawnPoint
			: spawnPoint = FindAnyObjectByType<PlayerSpawnPoint>();

		public Vector2 CachedInput { get; private set; }

		private void OnEnable()
		{
			var scale = visuals.localScale;
			visuals.localScale = Vector3.zero;
			visuals.DOScale(scale, 1f).SetEase(Ease.OutElastic);
			GoToSpawnPoint();
		}

		private void GoToSpawnPoint()
		{
			rb2d.transform.position = SpawnPoint.transform.position;
		}

		private void OnMove(InputValue value)
		{
			CachedInput = value.Get<Vector2>();
		}

		private void FixedUpdate()
		{
			if (rb2d != null)
			{
				rb2d.AddForce(movementSpeed * Time.fixedDeltaTime * CachedInput, ForceMode2D.Impulse);
			}
		}

		private void OnSplit()
		{
			OnPlayerSplit?.Invoke(playerContext);
		}
	}
}
