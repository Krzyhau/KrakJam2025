using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class MotherPlayerController : MonoBehaviour
	{
		public event Action<PlayerBubbleContext> OnPlayerAbsorbed;
		public event Action<PlayerBubbleContext> OnPlayerSplitted;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private MotherBubbleContext motherBubbleContext;

		[SerializeField]
		private MotherBubbleShapeManipulator shapeManipulator;

		[SerializeField]
		private LayerMask _motherBubbleLayer;

		[SerializeField]
		private LayerMask _bubbleLayer;

		private List<PlayerBubbleContext> playersInside = new();

		private const float SAFE_SPACE = 1;

		private void Subscribe(PlayerController player)
		{
			player.OnPlayerSplit += OnPlayerSplit;
		}

		private void Unsubscribe(PlayerController player)
		{
			player.OnPlayerSplit -= OnPlayerSplit;
		}

		public void AddPlayerInside(PlayerBubbleContext playerContext)
		{
			if (playersInside.Contains(playerContext))
			{
				return;
			}

			playersInside.Add(playerContext);
			Subscribe(playerContext.PlayerController);

			motherBubbleContext.WeightSystem.AddWeight(playerContext.WeightSystem.Weight);
			playerContext.WeightSystem.SetWeight(0);

			playerContext.Transform.SetParent(rb2d.transform, true);
			playerContext.Transform.localPosition = Vector2.zero;
			playerContext.Collider2D.excludeLayers = _bubbleLayer;

			OnPlayerAbsorbed?.Invoke(playerContext);
			motherBubbleContext.AudioSource.PlayOneShot(motherBubbleContext.AbsorbSound);
		}

		private void OnPlayerSplit(PlayerBubbleContext playerContext)
		{
			if (!playersInside.Contains(playerContext))
			{
				return;
			}
			Unsubscribe(playerContext.PlayerController);
			playersInside.Remove(playerContext);

			motherBubbleContext.WeightSystem.RemoveWeight(playerContext.WeightSystem.Weight);

			var inputDirection = playerContext.PlayerController.CachedInput != Vector2.zero
				? (Vector3)playerContext.PlayerController.CachedInput
				: playerContext.Transform.localPosition.normalized;

            var offsetFromMother = inputDirection * (shapeManipulator.TargetSize + playerContext.Transform.localScale.x * 0.8f);
			playerContext.Transform.localPosition = offsetFromMother;
			playerContext.Transform.SetParent(null);
			playerContext.Collider2D.excludeLayers = _motherBubbleLayer;

			OnPlayerSplitted?.Invoke(playerContext);

			motherBubbleContext.AudioSource.PlayOneShot(motherBubbleContext.SplitSound);
		}
	}
}
