using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Monke.KrakJam2025
{
    public class PanelIntroduction : MonoBehaviour
    {
        [SerializeField]
        private Image logo;

        [SerializeField]
        private CanvasGroup catTab;

        [SerializeField]
        private CanvasGroup hubTab;

        [SerializeField]
        private RectTransform leftBackground;

        [SerializeField]
        private RectTransform rightBackground;

        [SerializeField]
        private Transform catHandPivot;

        [SerializeField]
        private float fadeInDuration;

        [SerializeField]
        private float fadeOutDuration;

        [SerializeField]
        private float prependDuration;

        private void Awake()
        {
            var seq = DOTween.Sequence();

            hubTab.alpha = 0;
            catTab.alpha = 0;
            logo.color = new(1, 1, 1, 0);
            catHandPivot.eulerAngles = new(-187, -177, -140);

            seq.Insert(0, logo.DOFade(1, fadeInDuration))
                .PrependInterval(prependDuration)
                .Append(logo.DOFade(0, fadeOutDuration))
                .Append(catTab.DOFade(1, fadeInDuration))
                .Append(catHandPivot.DORotate(new(-187, -177, -50), fadeOutDuration))

                .SetLink(this.gameObject);

            var playerManager = FindAnyObjectByType<PlayerManagerInputHandler>();
            playerManager.OnPlayerJoin += OnPlayerJoin;
        }

        private void OnPlayerJoin()
        {
            var openSilloueteSequence = DOTween.Sequence()
                .Insert(0, catTab.DOFade(0, fadeOutDuration))
                .Append(leftBackground.DOMoveX(-1203, fadeOutDuration * 2))
                .Join(rightBackground.DOMoveX(1203, fadeOutDuration * 2)).SetLink(this.gameObject);
        }
    }
}
