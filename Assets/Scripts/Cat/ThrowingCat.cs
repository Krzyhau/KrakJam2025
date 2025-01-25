using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ThrowingCat : MonoBehaviour
	{
		[SerializeField]
		private CatAnimationNotifier _catAnimationNotifier;

		[Header("Throwing data")]
		[SerializeField]
		private float _timeToReachEndPoint = 1f;

		[SerializeField]
		private Transform _throwStartingPoint;

		[SerializeField]
		private Transform _throwDestinationPoint;

		private ItemSpawningSystem itemSpawningSystem;
		private StageParametersScriptableObject currentStageParameters;
		private CoroutineHandle throwingShitRoutine;
		private float currentTime = 0f;
		private bool hasThrowEnded;
		private BaseItem _itemToThrow;

		public void UpdateNewStageParameters(StageParametersScriptableObject newStageParameters)
		{
			currentStageParameters = newStageParameters;
		}

		public void StartThrowingShit()
		{
			Timing.KillCoroutines(throwingShitRoutine);
			throwingShitRoutine = Timing.RunCoroutine(ThrowingShitRoutine());
		}

		public void StopThrowingShit()
		{
			Timing.KillCoroutines(throwingShitRoutine);
		}

		private void Awake()
		{
			FetchComponents();
		}

		private void FetchComponents()
		{
			itemSpawningSystem = FindAnyObjectByType<ItemSpawningSystem>();
		}

		private void Start()
		{
			_catAnimationNotifier.OnThrowItemEvent += OnThrowItem;
			_catAnimationNotifier.OnThrowEndedEvent += OnThrowEnded;
		}

		private void OnThrowItem()
		{
			_itemToThrow.transform.parent = null;
			_itemToThrow.ThrowItem(_throwDestinationPoint.position, _timeToReachEndPoint);
		}

		private void OnThrowEnded()
		{
			hasThrowEnded = true;
		}

		private IEnumerator<float> ThrowingShitRoutine()
		{
			while (true)
			{
				hasThrowEnded = false;
				int randomIndex = Random.Range(0, currentStageParameters.PossibleItemsToSpawn.Count);
				var itemIdToSpawn = currentStageParameters.PossibleItemsToSpawn[randomIndex];
				_itemToThrow = itemSpawningSystem.SpawnObject(itemIdToSpawn, _throwStartingPoint);
				_catAnimationNotifier.InvokeThrowAnimation();

				while (!hasThrowEnded)
				{
					yield return Timing.WaitForOneFrame;
				}

				currentTime = 0f;
				var timeRange = currentStageParameters.TimeRangeForNewItem;
				var randomTimeToWait = Random.Range(timeRange.x, timeRange.y);

				while (currentTime < randomTimeToWait)
				{
					currentTime += Time.deltaTime;
					yield return Timing.WaitForOneFrame;
				}
			}
		}
	}
}
