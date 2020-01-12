using System;
using System.Linq;
using Expansions.Missions;
using Expansions.Missions.Adjusters;
using PreFlightTests;
using UnityEngine;

namespace AllAboard
{
    public class ModuleDockingPortAirlock : PartModule
    {
        public bool readyToReceive = false;
        private void FixedUpdate()
        {
            if (FlightGlobals.ActiveVessel == null || FlightGlobals.ActiveVessel.evaController == null) return;
            if (!InRange())
            {
                if(!readyToReceive)return;
                readyToReceive = false;
                DockingPortEvents.OnDockingPortPortUnreadyToBoard.Fire(this);
                return;
            }
            if (readyToReceive) return;
            if (PartWithCapacity() == null) return;
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

        public bool InRange()
        {
            var partTransform = part.transform;
            Vector3 directionToTarget = partTransform.position - FlightGlobals.ActiveVessel.transform.position;
            float angle = Vector3.Angle(-partTransform.up, directionToTarget);
            float distance = directionToTarget.magnitude;
            if (Mathf.Abs(angle) < 30 && distance < 10) return true;
            return false;
        }
    }
}