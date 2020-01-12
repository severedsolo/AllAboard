using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace AllAboard
{
    public class ModuleDockingPortAirlockEVA : PartModule
    {
        private ModuleDockingPortAirlock airlock;
        private void Start()
        {
            DockingPortEvents.OnDockingPortReadyToBoard.Add(RegisterDockingPort);
            DockingPortEvents.OnDockingPortPortUnreadyToBoard.Add(UnregisterDockingPort);
            Debug.Log("[DockingPortEntry]: ModuleDockingPortAirlockEVA : Start");
        }

        private void UnregisterDockingPort(ModuleDockingPortAirlock firingAirlock)
        {
            if (firingAirlock != airlock) return;
            airlock = null;
            Debug.Log("[DockingPortEntry]: ModuleDockingPortAirlockEVA : Unregistered port");
        }

        private void RegisterDockingPort(ModuleDockingPortAirlock firingAirlock)
        {
            airlock = firingAirlock;
            Debug.Log("[DockingPortEntry]: ModuleDockingPortAirlockEVA : Registered Port");
        }

        private void FixedUpdate()
        {
            if (airlock == null) return;
            if(Input.GetKeyDown(KeyCode.B))
            {
                if (!airlock.readyToReceive) return;
                if (airlock.PartWithCapacity() == null)
                {
                    Debug.Log("[DockingPortEntry]: Board aborted - no parts with capacity");
                    return;
                }
                Debug.Log("[DockingPortEntry]: Boarding Kerbal");
                vessel.evaController.BoardPart(airlock.PartWithCapacity());
            }
        }
    }
}