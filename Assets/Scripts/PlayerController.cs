using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<Vector2> OnPlayerMove;
        public event Action<PlayerController> OnPlayerSplit;

        [SerializeField]
        private float movementSpeed = 10f;

		[SerializeField]
		private Rigidbody2D rb2d;

		private Vector2 cachedInput;

        private void OnMove(InputValue value)
        {
            cachedInput = value.Get<Vector2>();
            OnPlayerMove?.Invoke(cachedInput);
        }

		private void FixedUpdate()
		{
			if (rb2d != null)
			{
				rb2d.AddForce(movementSpeed * Time.fixedDeltaTime * cachedInput, ForceMode2D.Impulse);
			}
		}

        private void OnSplit()
        {
            // do split deformation
            OnPlayerSplit?.Invoke(this);
        }
    }
}
