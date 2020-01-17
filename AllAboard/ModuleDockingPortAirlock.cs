using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;

namespace AllAboard
{
    public class ModuleDockingPortAirlock : PartModule
    {
        [KSPField(isPersistant = true, guiActive = false)]
        public float range = 1.0f;
        [KSPField(isPersistant = true, guiActive = false)]
        public string hatchDirection = "down";
        public bool readyToReceive;
        
        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeNullComparison")]
        private void FixedUpdate()
        {
            if (FlightGlobals.ActiveVessel == null || FlightGlobals.ActiveVessel.evaController == null) return;
            if (!InRange(FlightGlobals.ActiveVessel.evaController))
            {
                if(!readyToReceive)return;
                readyToReceive = false;
                DockingPortEvents.OnDockingPortPortUnreadyToBoard.Fire(this);
                return;
            }
            if (readyToReceive) return;
            if (!HasCapacity()) return;
            readyToReceive = true;
            ScreenMessages.PostScreenMessage("Press B to board through docking port");
            DockingPortEvents.OnDockingPortReadyToBoard.Fire(this);
        }

        public Part PartWithCapacity()
        {
            for (int i = 0; i < vessel.parts.Count; i++)
            {
                Part p = vessel.parts.ElementAt(i);
                if (p.CrewCapacity == 0) continue;
                if (p.CrewCapacity <= p.protoModuleCrew.Count) continue;
                return p;
            }

            return null;
        }

        public bool InRange(KerbalEVA kerbalOnEva)
        {
            Transform partTransform = part.transform;
            Vector3 directionToTarget = partTransform.position - kerbalOnEva.vessel.transform.position;
            float angle = Vector3.Angle(GetPortDirection(), directionToTarget);
            float distance = directionToTarget.magnitude;
#if DEBUG
            Debug.Log("Angle " + angle);
            Debug.Log("Range " + distance);
#endif
            if (Mathf.Abs(angle) < 35 && distance < range) return true;
            return false;
        }

        private Vector3 GetPortDirection()
        {
            switch(hatchDirection.ToLower())
            {
                case "up":
                    return part.transform.up;
                case "down":
                    return -part.transform.up;
                case "right":
                    return part.transform.right;
                case "left":
                    return -part.transform.right;
                case "forward":
                    return part.transform.forward;
                case "backwards":
                    return -part.transform.forward;
                default:
                    return -part.transform.up;
            }
        }

        public bool HasCapacity()
        {
            for (int i = 0; i < vessel.parts.Count; i++)
            {
                Part p = vessel.parts.ElementAt(i);
                if (p.CrewCapacity == 0) continue;
                if (p.CrewCapacity <= p.protoModuleCrew.Count) continue;
                return true;
            }
            return false;
        }
    }
}