using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class FlowSystem : MonoBehaviour
	{
		[SerializeField]
		private List<StageParametersScriptableObject> stageFlow;

		private int currentIndex = 0;
		private ThrowingCat throwingCat;

		private void Awake()
		{
			FetchComponents();
		}

		private void FetchComponents()
		{
			throwingCat = FindAnyObjectByType<ThrowingCat>();
		}

		private void StartGame()
		{

		}
	}
}
