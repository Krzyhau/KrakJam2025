using Rubin;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class AbsorbTrigger : MonoBehaviour
    {
        [SerializeField]
        private MotherPlayerController mother;

        [SerializeField]
        private float cooldown = 2;

        private Ticker ticker;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsReady(collision, out var playerTrigger))
            {
                mother.AddPlayerInside(playerTrigger.PlayerBubbleContext);
                ticker = TickerCreator.CreateNormalTime(cooldown);
                ticker.Reset();
            }
        }

        private bool IsReady(Collider2D collider, out PlayerTrigger playerTrigger)
        {
            playerTrigger = null;
            var state = ticker.Done && collider.gameObject.TryGetComponent(out playerTrigger) && playerTrigger.PlayerBubbleContext != null;
            return state;
        }
    }
}
