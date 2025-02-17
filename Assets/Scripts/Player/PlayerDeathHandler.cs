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

            // HACK
            // context.PlayerController.gameObject.SetActive(false);
            context.PlayerController.transform.position = new Vector3(-1000,-1000,-1000);

            OnDeathStarted?.Invoke();
            Timing.KillCoroutines(coroutineHandle);
            coroutineHandle = Timing.RunCoroutine(WaitForCooldown());
        }

        private IEnumerator<float> WaitForCooldown()
        {
            yield return Timing.WaitForSeconds(cooldownAfterDying);
            context.PlayerController.GoToSpawnPoint();
            //context.PlayerController.gameObject.SetActive(true);
            OnPlayerRespawned?.Invoke();
        }
    }
}
