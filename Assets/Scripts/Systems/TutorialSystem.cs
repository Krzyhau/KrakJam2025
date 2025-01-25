using System.Threading.Tasks;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class TutorialSystem : MonoBehaviour
	{
		private void StartTutorial()
		{

		}

		private async Task WaitForTutorialCompletion()
		{
			await Task.Yield();
		}
	}
}
