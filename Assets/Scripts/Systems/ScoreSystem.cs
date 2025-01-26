using Rubin;
using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ScoreSystem : MonoBehaviour
	{
		public event Action<float> OnScoreUpdated;
		public event Action<float> OnScoreAddedByTime;

		[SerializeField]
		private float cooldownToScore = 10;

		[SerializeField]
		private float scoreAdd = 10;

		private float _currentScore = 0f;
		private Ticker ticker;

		public float CooldownToScore => cooldownToScore;

		public float CurrentScore
		{
			get => _currentScore;
			set
			{
				if (_currentScore != value)
				{
					_currentScore = value;
					OnScoreUpdated?.Invoke(value);
				}
			}
		}

		public void AddScore(float score)
		{
			CurrentScore += score;
		}

		private void Awake()
		{
			ticker = TickerCreator.CreateNormalTime(cooldownToScore);
		}

		private void Update()
		{
			if (ticker.Push())
			{
				CurrentScore += scoreAdd;
				OnScoreAddedByTime?.Invoke(CurrentScore);
			}
		}
	}
}
