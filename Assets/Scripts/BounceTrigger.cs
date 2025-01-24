using UnityEngine;

namespace Monke.KrakJam2025
{
    public class BounceTrigger : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rb2d;

        [SerializeField]
        private float bounceForce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var bounceDirection = this.transform.position - collision.gameObject.transform.position;
            rb2d.AddForce(bounceDirection.normalized * bounceForce, ForceMode2D.Impulse);
        }
    }
}
