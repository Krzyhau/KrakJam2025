using Rubin;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class PlayerDeathTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsReady(collision, out var playerTrigger))
            {
                playerTrigger.PlayerBubbleContext.PlayerDeath.Death();
            }
        }

        private bool IsReady(Collider2D collider, out PlayerTrigger playerTrigger)
        {
            playerTrigger = null;
            var state = collider.gameObject.TryGetComponent(out playerTrigger) && playerTrigger.PlayerBubbleContext != null;
            return state;
        }
    }
}
