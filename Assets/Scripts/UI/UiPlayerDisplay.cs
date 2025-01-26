using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monke.KrakJam2025
{
    public class UiPlayerDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image backgroundFill;

        [SerializeField]
        private TextMeshProUGUI playerText;

        [SerializeField]
        private TextMeshProUGUI playerTextDuplicate;

        private PlayerDeathHandler deathHandler;

        private void Awake()
        {
            deathHandler = FindAnyObjectByType<PlayerDeathHandler>();
            deathHandler.OnDeathStarted += OnDeath;
            deathHandler.OnPlayerRespawned += OnPlayerRespawned;
        }

        private void OnDestroy()
        {
            deathHandler.OnDeathStarted -= OnDeath;
            deathHandler.OnPlayerRespawned -= OnPlayerRespawned;
        }

        private void OnPlayerRespawned()
        {
            backgroundFill.fillAmount = 0;
        }

        private void OnDeath()
        {
            backgroundFill.DOFillAmount(1, deathHandler.CooldownAfterDying)
                .SetLink(this.gameObject);
        }
    }
}
