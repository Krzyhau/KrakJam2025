using System.Collections.Generic;
using System.Linq;
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

		public void RegisterObject(VortexObject obj)
		{
			if (!registeredObjects.Contains(obj))
			{
				waitingToRegisterObjects.Add(obj);
			}
		}

		private void FixedUpdate()
		{
            registeredObjects.RemoveAll(x => waitingToDeregisterObjects.Contains(x));
            registeredObjects.AddRange(waitingToRegisterObjects);
            ClearWaitingLists();

            foreach (var vortexObject in registeredObjects)
			{
				Vector2 direction = (Vector2)vortexPoint.position - vortexObject.RigidBody.position;
				vortexObject.RigidBody.AddForce(direction.normalized * _vortexPower);
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
