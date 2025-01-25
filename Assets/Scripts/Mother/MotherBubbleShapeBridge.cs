using UnityEngine;

namespace Monke.KrakJam2025
{
	public class MotherBubbleShapeBridge : MonoBehaviour
	{
		[SerializeField]
		private MotherPlayerController controller;

		[SerializeField]
		private MotherBubbleShapeManipulator shapeManipulator;

		private void Awake()
		{
			controller.OnPlayerAbsorbed += OnPlayerAbsorbed;
			controller.OnPlayerSplitted += OnPlayerSplitted;
		}

		private void OnPlayerSplitted(PlayerBubbleContext obj)
		{
			shapeManipulator.SetStretcherState(obj.Transform, true);
		}

		private void OnPlayerAbsorbed(PlayerBubbleContext obj)
		{
			shapeManipulator.SetStretcherState(obj.Transform, false);
		}

		private void OnDestroy()
		{
			controller.OnPlayerAbsorbed -= OnPlayerAbsorbed;
			controller.OnPlayerSplitted -= OnPlayerSplitted;
		}
	}
}
