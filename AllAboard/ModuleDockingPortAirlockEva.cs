using UnityEngine;

namespace AllAboard
{
    public class ModuleDockingPortAirlockEva : PartModule
    {
        private ModuleDockingPortAirlock airlock;
        private bool isAirlockNull = true;

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
            isAirlockNull = true;
            Debug.Log("[DockingPortEntry]: ModuleDockingPortAirlockEVA : Unregistered port");
        }

        private void RegisterDockingPort(ModuleDockingPortAirlock firingAirlock)
        {
            airlock = firingAirlock;
            isAirlockNull = false;
            Debug.Log("[DockingPortEntry]: ModuleDockingPortAirlockEVA : Registered Port");
        }

        private void FixedUpdate()
        {
            if (isAirlockNull) return;
            if (!Input.GetKeyDown(KeyCode.B)) return;
            if (!airlock.readyToReceive || !airlock.InRange(vessel.evaController)) return;
            if (!airlock.HasCapacity())
            {
                // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
                Debug.Log("[DockingPortEntry]: Board aborted - no parts with capacity");
                return;
            }
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Debug.Log("[DockingPortEntry]: Boarding Kerbal");
            vessel.evaController.BoardPart(airlock.PartWithCapacity());
        }

        private void OnDisable()
        {
            DockingPortEvents.OnDockingPortReadyToBoard.Remove(RegisterDockingPort);
            DockingPortEvents.OnDockingPortPortUnreadyToBoard.Remove(UnregisterDockingPort);
        }
    }
}