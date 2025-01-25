using Rubin;
using System.Collections;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField]
        private float cooldownAfterDying;

        [SerializeField]
        private PlayerBubbleContext context;

        private Ticker ticker;

        public void Death()
        {
            context.PlayerController.gameObject.SetActive(false);
            ticker = TickerCreator.CreateNormalTime(cooldownAfterDying);
        }

        private void Update()
        {
            if (!ticker.Done)
            {
                return;
            }

            context.PlayerController.GoToSpawnPoint();
            context.PlayerController.gameObject.SetActive(true);
        }
    }
}
