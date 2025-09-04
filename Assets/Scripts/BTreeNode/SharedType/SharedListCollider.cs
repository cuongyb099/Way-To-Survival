using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace BTreeNode.SharedType
{
    [System.Serializable]
    public class SharedListCollider : SharedVariable<List<Collider>>
    {
        public static implicit operator SharedListCollider(List<Collider> value) 
        { return new SharedListCollider { mValue = value }; }
    }
}