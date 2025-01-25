using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ItemSpawningSystem : MonoBehaviour
	{
		[SerializeField]
		private ItemIdToPrefab _itemIdToPrefab;

		[Serializable]
		private struct ItemIdToPrefab
		{
			[SerializeField]
			private ItemIdScriptableObject _itemId;

			[SerializeField]
			private GameObject _itemPrefab;

			public ItemIdScriptableObject ItemId => _itemId;
			public GameObject ItemPrefab => _itemPrefab;
		}
	}
}
