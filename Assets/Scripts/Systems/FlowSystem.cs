using MEC;
using System;
using System.Collections.Generic;
using UnityEngine;

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

		private int currentIndex = 0;
		private ThrowingCat throwingCat;
		private CoroutineHandle _stageTimerHandle;

		private StageParametersScriptableObject CurrentStage => stageFlow[currentIndex];

		private void Awake()
		{
			FetchComponents();
		}

		private void FetchComponents()
		{
			throwingCat = FindAnyObjectByType<ThrowingCat>();
		}

		public void StartGame()
		{
			_stageTimerHandle = Timing.RunCoroutine(StagesRoutine());
		}

		private IEnumerator<float> StagesRoutine()
		{
			throwingCat.UpdateNewStageParameters(CurrentStage);
			throwingCat.StartThrowingShit();

			yield return Timing.WaitForSeconds(_timeBeforeStart);

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
