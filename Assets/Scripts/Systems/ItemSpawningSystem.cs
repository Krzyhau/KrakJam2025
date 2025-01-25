using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ItemSpawningSystem : MonoBehaviour
	{
		[SerializeField]
		private List<ItemIdToPrefab> _itemsIdToPrefab;

		public GameObject SpawnObject(ItemIdScriptableObject itemId, Transform parent)
		{
			var itemIdToPrefab = _itemsIdToPrefab.Find(x => x.ItemId == itemId);
			var itemPrefab = itemIdToPrefab.ItemPrefab;
			var newObject = Instantiate(itemPrefab, parent, false);
			newObject.transform.parent = null;
			return newObject;
		}

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
