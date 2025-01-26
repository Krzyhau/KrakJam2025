using UnityEngine;

namespace Monke.KrakJam2025
{
	public class CameraSystem : MonoBehaviour
	{
		[SerializeField]
		private Camera _camera;

		public Camera Camera => _camera;
	}
}
