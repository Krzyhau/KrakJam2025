using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed = 10f;

        [SerializeField]
        private Rigidbody2D rb2d;

        private Vector2 cachedInput;

        private void OnMove(InputValue value)
        {
            cachedInput = value.Get<Vector2>();
        }

        private void FixedUpdate()
        {
            if (rb2d != null)
            {
                rb2d.MovePosition(rb2d.position + (movementSpeed * Time.fixedDeltaTime * cachedInput));
            }
        }

        private void OnInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}
