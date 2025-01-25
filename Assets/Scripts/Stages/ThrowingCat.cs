using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ThrowingCat : MonoBehaviour
	{
		[SerializeField]
		private Transform _throwStartingPoint;

		private ItemSpawningSystem itemSpawningSystem;
		private StageParametersScriptableObject currentStageParameters;
		private CoroutineHandle throwingShitRoutine;
		private float currentTime = 0f;

		public void UpdateNewStageParameters(StageParametersScriptableObject newStageParameters)
		{
			currentStageParameters = newStageParameters;
		}

		public void StartThrowingShit()
		{
			Timing.KillCoroutines(throwingShitRoutine);
			throwingShitRoutine = Timing.RunCoroutine(ThrowingShitRoutine());
		}

		private void Awake()
		{
			FetchComponents();
		}

		private IEnumerator<float> ThrowingShitRoutine()
		{
			while (true)
			{
				int randomIndex = Random.Range(0, currentStageParameters.PossibleItemsToSpawn.Count);
				var itemIdToSpawn = currentStageParameters.PossibleItemsToSpawn[randomIndex];
				var spawnedItem = itemSpawningSystem.SpawnObject(itemIdToSpawn, _throwStartingPoint);

				// TODO: handle throw animation

				var currentTime = 0f;
				var timeRange = currentStageParameters.TimeRangeForNewItem;
				var randomTimeToWait = Random.Range(timeRange.x, timeRange.y);

				while (currentTime < randomTimeToWait)
				{
					currentTime += Time.deltaTime;
					yield return Timing.WaitForOneFrame;
				}
			}
		}

		private void FetchComponents()
		{
			itemSpawningSystem = FindAnyObjectByType<ItemSpawningSystem>();
		}
	}
}
