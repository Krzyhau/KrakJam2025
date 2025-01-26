using UnityEngine;

namespace Monke.KrakJam2025
{
	public class LookAtCamera : MonoBehaviour
	{
		private CameraSystem _cameraSystem;
		private bool _updateLookAt;
		[SerializeField] private bool reverse;

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
				var lookAtDir = (_cameraSystem.Camera.transform.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(lookAtDir * (reverse ? -1 : 1), Vector3.back);
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
