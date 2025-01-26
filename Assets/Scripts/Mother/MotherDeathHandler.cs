using UnityEngine;

namespace Monke.KrakJam2025
{
	public class MotherDeathHandler : MonoBehaviour
	{
		private FlowSystem _flowSystem;

		private void Awake()
		{
			_flowSystem = FindAnyObjectByType<FlowSystem>();
		}

		public void TriggerDeath()
		{
			Debug.Log("KONIEC GRY");
			_flowSystem.EndGame();
			// TO DO: Add additional mechanics on triggering end game
		}
	}
}
