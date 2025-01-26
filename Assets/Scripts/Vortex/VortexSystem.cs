using System.Collections.Generic;
using UnityEngine;

namespace Monke.KrakJam2025
{
	public class VortexSystem : MonoBehaviour
	{
		[SerializeField]
		private Transform vortexPoint;

		[SerializeField]
		private float _vortexPower = 2;

		private List<VortexObject> registeredObjects = new();
		private readonly HashSet<VortexObject> waitingToRegisterObjects = new();
		private readonly HashSet<VortexObject> waitingToDeregisterObjects = new();
		private bool _isVortexActive = false;

		public void RegisterObject(VortexObject obj)
		{
			if (!registeredObjects.Contains(obj))
			{
				waitingToRegisterObjects.Add(obj);
			}
		}

		public void SetVortexActive(bool active)
		{
			_isVortexActive = active;
		}

		private void FixedUpdate()
		{
			if (_isVortexActive)
			{
				registeredObjects.RemoveAll(x => waitingToDeregisterObjects.Contains(x));
				registeredObjects.AddRange(waitingToRegisterObjects);
				ClearWaitingLists();

				foreach (var vortexObject in registeredObjects)
				{
					Vector2 direction = (Vector2)vortexPoint.position - vortexObject.RigidBody.position;
					vortexObject.RigidBody.AddForce(_vortexPower * vortexObject.SpeedMultiplier * direction.normalized);
				}
			}
		}

		private void ClearWaitingLists()
		{
			waitingToDeregisterObjects.Clear();
			waitingToRegisterObjects.Clear();
		}

		public void DeregisterObject(VortexObject obj)
		{
			if (registeredObjects.Contains(obj))
			{
				waitingToDeregisterObjects.Add(obj);
			}
		}

		private void OnDestroy()
		{
			ClearWaitingLists();
			registeredObjects.Clear();
		}
	}
}
