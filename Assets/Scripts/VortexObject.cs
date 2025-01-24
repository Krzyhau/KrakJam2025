using UnityEngine;

namespace Monke.KrakJam2025
{
	public class VortexObject : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private bool registerOnStart = true;

		private VortexSystem vortexSystem;

		public Rigidbody2D RigidBody => rb2d;

		private void Awake()
		{
			vortexSystem = FindFirstObjectByType<VortexSystem>();
		}

		private void Start()
		{
			if (registerOnStart)
			{
				Register();
			}
		}

		private void Register()
		{
			vortexSystem.RegisterObject(this);
		}

		private void Deregister()
		{
			if (vortexSystem != null)
			{
				vortexSystem.DeregisterObject(this);
			}
		}

		private void OnDestroy()
		{
			Deregister();
		}
	}
}
