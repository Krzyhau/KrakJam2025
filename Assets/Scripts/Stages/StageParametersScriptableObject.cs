using UnityEngine;

namespace Monke.KrakJam2025
{
	[CreateAssetMenu(fileName = "StageParameters_Level", menuName = "Scriptable Objects/StageParameters")]
	public class StageParametersScriptableObject : ScriptableObject
	{
		[SerializeField]
		private Vector2 _timeRangeForNewItem = Vector2.zero;

		[SerializeField]
		private ItemIdScriptableObject _possibleItemsToSpawn;
	}
}
