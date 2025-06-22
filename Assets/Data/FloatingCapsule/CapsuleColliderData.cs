using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{
    [Serializable]
    public class CapsuleColliderData
    {
        [field: SerializeField] public CapsuleCollider Collider { get; private set; }
        public Vector3 ColliderCenterLocalSpace { get; private set; }
        public void Initialize()
        {
            UpdateColliderData();
        }
        public void UpdateColliderData()
        {
            ColliderCenterLocalSpace = Collider.center;
        }
    }
}
