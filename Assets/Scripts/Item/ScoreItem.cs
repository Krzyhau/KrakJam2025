using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ScoreItem : BaseItem
	{
		private ScoreSystem _scoreSystem;

		[SerializeField]
		private float _scorePoints = 30;

		private void Awake()
		{
			_scoreSystem = FindAnyObjectByType<ScoreSystem>();
		}

		protected override void OnBubbleCollided()
		{
			_scoreSystem.AddScore(_scorePoints);
			Destroy(gameObject);
		}
	}
}
