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
            if (ticker.Done && collision.gameObject.TryGetComponent(out PlayerTrigger playerTrigger) && playerTrigger.PlayerBubbleContext != null)
            {
                mother.AddPlayerInside(playerTrigger.PlayerBubbleContext);
                ticker = TickerCreator.CreateNormalTime(cooldown);
                ticker.Reset();
            }
        }
    }
}
