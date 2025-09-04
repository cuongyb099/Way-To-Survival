using Core.Animation_Event_System;
using Core.Animation_Event_System.Event_Data;

namespace DefaultNamespace.Entity.Enemy
{
    public class ZombieAnimEvent : AnimEvent<ZombieAnimEventID>
    {
       
    }

    public enum ZombieAnimEventID
    {
        Attack,
        AttackDone,
    }
}