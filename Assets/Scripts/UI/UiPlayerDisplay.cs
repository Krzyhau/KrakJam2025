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

        public void Setup(string name, Color color)
        {
            playerText.SetText(name);
            playerTextDuplicate.SetText(name);

            backgroundFill.color = color;
            backgroundFill.fillAmount = 1;
        }

        private void OnPlayerRespawned()
        {
            backgroundFill.fillAmount = 1;
        }

        private void OnDeath()
        {
            Debug.Log("DEATH");

            backgroundFill.DOFillAmount(0, deathHandler.CooldownAfterDying)
                .SetLink(this.gameObject);
        }
    }
}
