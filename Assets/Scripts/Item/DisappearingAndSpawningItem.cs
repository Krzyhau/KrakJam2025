using MEC;
using Rubin;
using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class DisappearingAndSpawningItem : BaseItem
	{
		[SerializeField]
		private GameObject _itemToSpawn;

		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[SerializeField]
		private float _timeToZeroAlpha = 1.0f;

		[SerializeField]
		private int _numberOfItems = 3;

		[SerializeField]
		private float _spawnRadius = 0.5f;

		protected override void OnThrowEnded()
		{
			Timing.RunCoroutine(AlphaToZeroRoutine());
		}

		private IEnumerator<float> AlphaToZeroRoutine()
		{
			float currentTime = 0;

			while (currentTime < _timeToZeroAlpha)
			{
				currentTime += Time.deltaTime;
				var color = _spriteRenderer.color;
				color.a = 1 - currentTime / _timeToZeroAlpha;
				_spriteRenderer.color = color;
				yield return Timing.WaitForOneFrame;
			}

			_spriteRenderer.color.WithAlpha(0);
			SpawnItems();
			Destroy(gameObject);
		}

		private void SpawnItems()
		{
			for (int i = 0; i < _numberOfItems; i++)
			{
				Vector3 spawnPosition = new Vector3(
					transform.position.x + Random.Range(-_spawnRadius, _spawnRadius),
					transform.position.y + Random.Range(-_spawnRadius, _spawnRadius),
					transform.position.z + Random.Range(-_spawnRadius, _spawnRadius)
				);

				Instantiate(_itemToSpawn, spawnPosition, Quaternion.identity);
			}
		}
	}
}
