using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace BTreeNode.SharedType
{
    [System.Serializable]
    public class SharedHashsetTransform : SharedVariable<HashSet<Transform>>
    {
        public static implicit operator SharedHashsetTransform(HashSet<Transform> value) 
        { return new SharedHashsetTransform { mValue = value }; }
    }
}