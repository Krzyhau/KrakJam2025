using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private float movementSpeed;

        private void OnMove(InputValue value)
        {
            var vect = value.Get<Vector2>();
            this.transform.Translate(vect * Time.deltaTime * movementSpeed);
        }

        private void OnInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}
