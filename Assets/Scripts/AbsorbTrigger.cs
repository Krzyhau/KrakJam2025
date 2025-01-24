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
            player.ChangeMovement(false);
            mother.AddPlayerInside(player);
            player.transform.SetParent(mother.Rb2D.transform, false);
            player.transform.localPosition = Vector2.zero;
        }
    }
}
