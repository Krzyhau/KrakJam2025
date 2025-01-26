using MEC;
using Rubin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        [SerializeField]
        private float cooldownAfterDying;

        [SerializeField]
        private PlayerBubbleContext context;

        private CoroutineHandle coroutineHandle;

        public void Death()
        {
            context.PlayerController.gameObject.SetActive(false);

            Timing.KillCoroutines(coroutineHandle);
            coroutineHandle = Timing.RunCoroutine(WaitForCooldown());
        }

        private IEnumerator<float> WaitForCooldown()
        {
            yield return Timing.WaitForSeconds(cooldownAfterDying);
            context.PlayerController.GoToSpawnPoint();
            context.PlayerController.gameObject.SetActive(true);
        }
    }
}
