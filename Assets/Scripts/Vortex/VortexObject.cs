using UnityEngine;

namespace Monke.KrakJam2025
{
	public class VortexObject : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody2D rb2d;

		[SerializeField]
		private float speedMultiplier = 1;

		[SerializeField]
		private bool registerOnStart = true;

		private VortexSystem vortexSystem;

		public Rigidbody2D RigidBody => rb2d;
		public float SpeedMultiplier => speedMultiplier;

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

		public void Register()
		{
			if (vortexSystem == null)
			{
				return;
			}
			vortexSystem.RegisterObject(this);
		}

		public void Deregister()
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
