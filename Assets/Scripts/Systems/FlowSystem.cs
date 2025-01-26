using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Monke.KrakJam2025
{
	public class FlowSystem : MonoBehaviour
	{
		public event Action OnGameFinished;

		[SerializeField]
		private List<StageParametersScriptableObject> stageFlow;

		[SerializeField]
		private float _timeBetweenStages = 5;

		[SerializeField]
		private float _timeBeforeStart = 5;

		private PlayerInputManager _playerInputManager;
		private int currentIndex = 0;
		private ThrowingCat throwingCat;
		private CoroutineHandle _stageTimerHandle;
		private VortexSystem _vortexSystem;

		private StageParametersScriptableObject CurrentStage => stageFlow[currentIndex];

		private void Awake()
		{
			FetchComponents();
		}

		private void FetchComponents()
		{
			throwingCat = FindAnyObjectByType<ThrowingCat>();
			_playerInputManager = FindAnyObjectByType<PlayerInputManager>();
			_vortexSystem = FindAnyObjectByType<VortexSystem>();
		}

		public void StartGame()
		{
			_playerInputManager.DisableJoining();
			_stageTimerHandle = Timing.RunCoroutine(StagesRoutine());
			_vortexSystem.SetVortexActive(true);
		}

		private IEnumerator<float> StagesRoutine()
		{
			yield return Timing.WaitForSeconds(_timeBeforeStart);

			throwingCat.UpdateNewStageParameters(CurrentStage);
			throwingCat.StartThrowingShit();

			while (true)
			{
				yield return Timing.WaitForSeconds(CurrentStage.StageTime);
				currentIndex++;
				throwingCat.StopThrowingShit();

				yield return Timing.WaitForSeconds(_timeBetweenStages);

				if (currentIndex == stageFlow.Count)
				{
					EndGame();
				}

				throwingCat.UpdateNewStageParameters(CurrentStage);
				throwingCat.StartThrowingShit();
			}
		}

		public void EndGame()
		{
			throwingCat.StopThrowingShit();
			Timing.KillCoroutines(_stageTimerHandle);

			OnGameFinished?.Invoke();

        }
	}
}
