using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
	public class PlayerController : MonoBehaviour
	{
        public event Action<PlayerBubbleContext> OnPlayerSplit;
		public event Action OnPlayerStart;

        [Header("Visuals")]
		[SerializeField] private Transform visuals;
		[SerializeField] private ParticleSystem trailParticles;
		[SerializeField] private MeshRenderer baseMesh;

        [Header("Properties")]
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

		public float MovementSpeed
		{
			get => movementSpeed;
			set => movementSpeed = value;
		}

		private void OnEnable()
		{
			var scale = visuals.localScale;
			var bubbleShapeManipulator = FindAnyObjectByType<MotherBubbleShapeManipulator>();
			bubbleShapeManipulator.SetStretcherState(transform, true);
			visuals.localScale = Vector3.zero;
			visuals.DOScale(scale, 1f).SetEase(Ease.OutElastic);
			GoToSpawnPoint();
		}

		public void GoToSpawnPoint()
		{
			rb2d.transform.position = SpawnPoint.transform.position;
		}

		public void SetPlayerColor(Color c)
		{
            var settings = trailParticles.main;
            settings.startColor = c;

            baseMesh.material.color = c;
        }

		private void OnMove(InputValue value)
		{
			CachedInput = value.Get<Vector2>();
		}

		private void FixedUpdate()
		{
			if (rb2d != null)
			{
				rb2d.AddForce(MovementSpeed * Time.fixedDeltaTime * CachedInput, ForceMode2D.Impulse);
			}
		}

		private void OnSplit()
		{
			OnPlayerSplit?.Invoke(playerContext);
		}

		private void OnStart()
		{
			OnPlayerStart?.Invoke();
		}
	}
}
