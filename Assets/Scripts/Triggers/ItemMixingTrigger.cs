using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	[RequireComponent(typeof(Collider2D))]
	public class ItemMixingTrigger : MonoBehaviour
	{
		private readonly List<BaseItem> _itemsOnOrbit = new();
		private readonly List<BaseItem> _itemsToRemove = new();
		private readonly List<BaseItem> _itemsToAdd = new();

		[SerializeField]
		private Transform _orbitSource;

		[SerializeField]
		private Transform _maxLeftPoint;

		[SerializeField]
		private Transform _maxRightPoint;

		[SerializeField]
		private Vector2 _mixingTimeRange = Vector2.one;

		[SerializeField]
		private float _orbitRadius = 5f;

		[SerializeField]
		private float _forceStrength = 10f;

		[SerializeField]
		private LayerMask _mixingTriggerLayerMask;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.TryGetComponent(out BaseItem item) && !_itemsOnOrbit.Contains(item) && !_itemsToAdd.Contains(item))
			{
				_itemsToAdd.Add(item);
				item.Collider2D.excludeLayers = _mixingTriggerLayerMask;
				Timing.RunCoroutine(EndMixing(item));
			}
		}

		private IEnumerator<float> EndMixing(BaseItem item)
		{
			yield return Timing.WaitForSeconds(Random.Range(_mixingTimeRange.x, _mixingTimeRange.y));

			_itemsToRemove.Add(item);
		}

		private void FixedUpdate()
		{
			RemoveAndCatapultItems();

			_itemsToAdd.ForEach(item => _itemsOnOrbit.Add(item));
			_itemsToAdd.Clear();

			var time = Time.deltaTime;

			foreach (var item in _itemsOnOrbit)
			{
				if (item != null)
				{
					Vector2 directionToTarget = (Vector2)_orbitSource.position - item.Rigidbody2D.position;
					var orbitDirection = Vector3.Cross(directionToTarget, Vector3.forward).normalized;
					item.Rigidbody2D.AddForce(orbitDirection * _forceStrength);

					var sgn = directionToTarget.magnitude > _orbitRadius ? -1 : 1;
					item.Rigidbody2D.AddForce(directionToTarget * _forceStrength * sgn);
				}
				else
				{
					_itemsToRemove.Add(item);
				}
			}
		}

		private void RemoveAndCatapultItems()
		{
			foreach (var itemToRemove in _itemsToRemove)
			{
				if (itemToRemove != null && _itemsOnOrbit.Contains(itemToRemove))
				{
					_itemsOnOrbit.Remove(itemToRemove);
					float random = Random.Range(0f, 1f);
					Vector3 randomVector = Vector3.Lerp
						(
						_maxLeftPoint.position - _orbitSource.position,
						_maxRightPoint.position - _orbitSource.position,
						random
						);
					itemToRemove.Rigidbody2D.AddForce(
						randomVector.normalized * itemToRemove.CatapultStrength, ForceMode2D.Impulse);
					itemToRemove.RegisterToVortex();
				}
			}

			_itemsToRemove.Clear();
		}
	}
}
