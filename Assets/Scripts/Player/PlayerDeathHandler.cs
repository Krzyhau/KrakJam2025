using MEC;
using Rubin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
    public class PlayerDeathHandler : MonoBehaviour
    {
        public event Action OnDeathStarted;
        public event Action OnPlayerRespawned;

        [SerializeField]
        private float cooldownAfterDying;

        [SerializeField]
        private PlayerBubbleContext context;

        private CoroutineHandle coroutineHandle;

        public float CooldownAfterDying => cooldownAfterDying;

        public void Death()
        {
            context.PlayerController.gameObject.SetActive(false);
            OnDeathStarted?.Invoke();

            Timing.KillCoroutines(coroutineHandle);
            coroutineHandle = Timing.RunCoroutine(WaitForCooldown().CancelWith(gameObject));
        }

        private IEnumerator<float> WaitForCooldown()
        {
            yield return Timing.WaitForSeconds(cooldownAfterDying);
            context.PlayerController.GoToSpawnPoint();
            context.PlayerController.gameObject.SetActive(true);
            OnPlayerRespawned?.Invoke();
        }
    }
}
