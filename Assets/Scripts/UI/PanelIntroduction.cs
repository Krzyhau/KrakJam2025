using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
		private CanvasGroup hudTab;

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

		[SerializeField]
		private TextMeshProUGUI signText;

		private Sequence seq;
		private FlowSystem flowSystem;
		private List<PlayerController> _joinedPlayers = new();

		private float leftPositionX;
		private float rightPositionX;

		private void Awake()
		{
			flowSystem = FindAnyObjectByType<FlowSystem>();

			hubTab.alpha = 0;
			catTab.alpha = 0;
			logo.color = new(1, 1, 1, 0);

			leftPositionX = leftBackground.position.x;
			rightPositionX = rightBackground.position.x;

			var playerManager = FindAnyObjectByType<PlayerManagerInputHandler>();
			playerManager.OnPlayerJoin += OnPlayerJoin;
			flowSystem.OnGameFinished += OnGameFinished;

			seq = DOTween.Sequence().Insert(0, logo.DOFade(1, fadeInDuration))
				.PrependInterval(prependDuration)
				.Append(logo.DOFade(0, fadeOutDuration))
				.Append(catTab.DOFade(1, fadeInDuration))
				.Append(GetCatHand())
				.SetLink(this.gameObject).SetUpdate(true);
		}

		private Tween GetCatHand()
		{
			catHandPivot.eulerAngles = new(-187, -177, -140);

			return catHandPivot.DORotate(new(0, 0, 140), fadeOutDuration);
		}

		private void OnGameFinished()
		{
            var gameOverSystem = FindAnyObjectByType<GameOverSystem>();
			gameOverSystem.StartGameOver();
            /*
            Time.timeScale = 0;

            leftBackground.position = new(leftPositionX, leftBackground.position.y);
            rightBackground.position = new(rightPositionX, rightBackground.position.y);

            hudTab.alpha = 0;
            catTab.alpha = 0;

            var t = DOTween.Sequence().Insert(0, catTab.DOFade(1, fadeInDuration))
                .Append(GetCatHand()).SetLink(this.gameObject).SetUpdate(true);

            signText.SetText("GAME OVER\nPress any button to continue...");
            */
        }

		private void OnPlayerJoin(PlayerController playerController)
		{
			_joinedPlayers.Add(playerController);
			playerController.OnPlayerStart += OnPlayerStart;
		}

		private void OnPlayerStart()
		{
			_joinedPlayers.ForEach(player => player.OnPlayerStart -= OnPlayerStart);
			seq?.Kill();

            logo.color = new(1, 1, 1, 0);

			var openSilloueteSequence = DOTween.Sequence()
				.Insert(0, catTab.DOFade(0, fadeOutDuration / 2))
				.Insert(0, leftBackground.DOMoveX(-2000, fadeOutDuration * 3))
				.Insert(0, rightBackground.DOMoveX(2000, fadeOutDuration * 3)).SetLink(this.gameObject).SetUpdate(true);

			flowSystem.StartGame();
		}
	}
}
