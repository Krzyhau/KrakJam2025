using UnityEngine;

namespace Monke.KrakJam2025
{
    public class AbsorbTrigger : MonoBehaviour
    {
        [SerializeField]
        private MotherPlayerController mother;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponentInParent<PlayerController>();
            mother.AddPlayerInside(player);
        }
    }
}
