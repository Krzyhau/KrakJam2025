using System;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ItemSpawningSystem : MonoBehaviour
	{
		[SerializeField]
		private List<ItemIdToPrefab> _itemsIdToPrefab;

		public BaseItem SpawnObject(ItemIdScriptableObject itemId, Transform parent)
		{
			var itemIdToPrefab = _itemsIdToPrefab.Find(x => x.ItemId == itemId);
			var itemPrefab = itemIdToPrefab.ItemPrefab;
			var instantiatedGameObject = Instantiate(itemPrefab, parent);
			instantiatedGameObject.transform.localPosition = Vector3.zero;
			return instantiatedGameObject.GetComponent<BaseItem>();
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
