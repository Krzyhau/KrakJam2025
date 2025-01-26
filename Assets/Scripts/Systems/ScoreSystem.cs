using System;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class ScoreSystem : MonoBehaviour
	{
		public event Action<float> OnScoreUpdated;

		private float _currentScore = 0f;

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
	}
}
