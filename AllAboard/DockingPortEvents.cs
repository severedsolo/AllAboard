using System;
using UnityEngine;

namespace AllAboard
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class DockingPortEvents : MonoBehaviour
    {
        public static EventData<ModuleDockingPortAirlock>OnDockingPortReadyToBoard;
        public static EventData<ModuleDockingPortAirlock>OnDockingPortPortUnreadyToBoard;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            OnDockingPortReadyToBoard = new EventData<ModuleDockingPortAirlock>("OnDockingPortReadyToBoard");
            OnDockingPortPortUnreadyToBoard = new EventData<ModuleDockingPortAirlock>("OnDockingPortUnreadyToBoard");
        }
    }
}