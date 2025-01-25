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

        private bool shouldMove = true;

        public void ChangeMovement(bool shouldMovePlayer)
        {
            shouldMove = shouldMovePlayer;
            rb2d.bodyType = shouldMovePlayer ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            rb2d.simulated = shouldMovePlayer;
        }

        private void OnMove(InputValue value)
        {
            CachedInput = value.Get<Vector2>();
        }

		private void FixedUpdate()
		{
			if (rb2d != null && shouldMove)
			{
				rb2d.AddForce(movementSpeed * Time.fixedDeltaTime * CachedInput, ForceMode2D.Impulse);
			}
		}

        private void OnSplit()
        {
            // do split deformation
            OnPlayerSplit?.Invoke(playerContext);
        }
    }
}
