using UnityEngine;

namespace Monke.KrakJam2025
{
    public class AbsorbTrigger : MonoBehaviour
    {
        [SerializeField]
        private MotherPlayerController mother;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out PlayerTrigger playerTrigger) && playerTrigger.PlayerBubbleContext != null)
            {
                mother.AddPlayerInside(playerTrigger.PlayerBubbleContext);
            }
        }
    }
}
