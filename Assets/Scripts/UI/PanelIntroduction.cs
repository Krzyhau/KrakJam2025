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

        private Sequence seq;
        private FlowSystem flowSystem;

        private void Awake()
        {
            flowSystem = FindAnyObjectByType<FlowSystem>();

            seq = DOTween.Sequence();

            hubTab.alpha = 0;
            catTab.alpha = 0;
            logo.color = new(1, 1, 1, 0);
            catHandPivot.eulerAngles = new(-187, -177, -140);

            seq.Insert(0, logo.DOFade(1, fadeInDuration))
                .PrependInterval(prependDuration)
                .Append(logo.DOFade(0, fadeOutDuration))
                .Append(catTab.DOFade(1, fadeInDuration))
                .Append(catHandPivot.DORotate(new(0, 0, 90), fadeOutDuration))
                .SetLink(this.gameObject);

            var playerManager = FindAnyObjectByType<PlayerManagerInputHandler>();
            playerManager.OnPlayerJoin += OnPlayerJoin;
        }

        private void OnPlayerJoin()
        {
            seq?.Kill(true);
            var openSilloueteSequence = DOTween.Sequence()
                .Insert(0, catTab.DOFade(0, fadeOutDuration / 2))
                .Insert(0, leftBackground.DOMoveX(-1203, fadeOutDuration * 2.5f))
                .Insert(0, rightBackground.DOMoveX(1203, fadeOutDuration * 2.5f)).SetLink(this.gameObject);

            flowSystem.StartGame();
        }
    }
}
