using System;
using Core.Animation_Event_System;
using Core.Animation_Event_System.Event_Data;
using Unity.VisualScripting;

namespace DefaultNamespace.Buliding.Turret
{
    [System.Serializable]
    public class TurretAnimEvent : AnimEvent<TurretAnimEventID>
    {
        
    }
    
    [System.Serializable]
    public class TurretAnimIntEvent : AnimEvent<int, TurretAnimEventID>
    {
        
    }
    
    public enum TurretAnimEventID
    {
        ShotDone, //Param : int : gun index
        Shot,
    }
}