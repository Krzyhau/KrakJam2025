using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PlayerController> OnPlayerSplit;

        [SerializeField]
        private float movementSpeed = 10f;

		[SerializeField]
		private Rigidbody2D rb2d;

		public Vector2 CachedInput { get; private set; }

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
            // do split deformation
            OnPlayerSplit?.Invoke(this);
        }
    }
}
