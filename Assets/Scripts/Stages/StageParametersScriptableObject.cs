using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	[CreateAssetMenu(fileName = "StageParameters_Level", menuName = "Scriptable Objects/StageParameters")]
	public class StageParametersScriptableObject : ScriptableObject
	{
		[SerializeField]
		private Vector2 _timeRangeForNewItem = Vector2.zero;

		[SerializeField]
		private float _stageTime = 60;

		[SerializeField]
		private List<ItemIdScriptableObject> _possibleItemsToSpawn;

		public Vector2 TimeRangeForNewItem => _timeRangeForNewItem;
		public float StageTime => _stageTime;
		public List<ItemIdScriptableObject> PossibleItemsToSpawn => _possibleItemsToSpawn;
	}
}
