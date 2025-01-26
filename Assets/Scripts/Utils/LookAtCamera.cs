using UnityEngine;

namespace Monke.KrakJam2025
{
	public class LookAtCamera : MonoBehaviour
	{
		private CameraSystem _cameraSystem;
		private bool _updateLookAt;

		private void Awake()
		{
			_cameraSystem = FindAnyObjectByType<CameraSystem>();
		}

		private void Start()
		{
			_updateLookAt = true;
		}

		private void FixedUpdate()
		{
			if (_updateLookAt)
			{
				transform.LookAt(_cameraSystem.Camera.transform, Vector3.back);
			}
		}

		private void OnDisable()
		{
			_updateLookAt = false;
		}

		private void OnDestroy()
		{
			_updateLookAt = false;
		}
	}
}
