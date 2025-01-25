using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
	public class PlayerController : MonoBehaviour
	{
		public event Action<PlayerBubbleContext> OnPlayerSplit;

		[SerializeField]
		private float movementSpeed = 10f;

		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private PlayerBubbleContext playerContext;

		public Vector2 CachedInput { get; private set; }

		private bool isMotherNotControlling = true;

		public void ChangeMovement(bool isMotherNotControlling)
		{
			this.isMotherNotControlling = isMotherNotControlling;
			//rb2d.bodyType = isMotherNotControlling ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
			//rb2d.simulated = isMotherNotControlling;
		}

		private void OnMove(InputValue value)
		{
			CachedInput = value.Get<Vector2>();
		}

		private void FixedUpdate()
		{
			if (rb2d != null)
			{
				if (isMotherNotControlling)
				{
					rb2d.AddForce(movementSpeed * Time.fixedDeltaTime * CachedInput, ForceMode2D.Impulse);

				}
				else
				{
					rb2d.AddForce(movementSpeed * Time.fixedDeltaTime * CachedInput, ForceMode2D.Impulse);
				}
			}
		}

		private void OnSplit()
		{
			if (!isMotherNotControlling)
			{
				OnPlayerSplit?.Invoke(playerContext);
			}
		}
	}
}
