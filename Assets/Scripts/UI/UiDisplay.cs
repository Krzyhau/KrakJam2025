using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Monke.KrakJam2025
{
    public class UiDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image timeFill;

        [SerializeField]
        private TextMeshProUGUI scoreText;

        private ScoreSystem scoreSystem;
        private Tween tween;

        private void Awake()
        {
            scoreSystem = FindAnyObjectByType<ScoreSystem>();
            scoreSystem.OnScoreUpdated += OnScoreUpdated;
            scoreSystem.OnScoreAddedByTime += OnScoreAddedByTime;

            if (tween.IsActive())
            {
                return;
            }
            tween = timeFill.DOFillAmount(1, scoreSystem.CooldownToScore)
                .SetLink(this.gameObject);
        }

        private void OnDestroy()
        {
            scoreSystem.OnScoreUpdated -= OnScoreUpdated;
            scoreSystem.OnScoreAddedByTime -= OnScoreAddedByTime;
        }

        private void OnScoreAddedByTime(float score)
        {
            tween.Kill();
            timeFill.fillAmount = 0;
            tween = timeFill.DOFillAmount(1, scoreSystem.CooldownToScore)
                .SetLink(this.gameObject);
        }

        private void OnScoreUpdated(float score)
        {
            scoreText.SetText(score.ToString());
        }
    }
}
