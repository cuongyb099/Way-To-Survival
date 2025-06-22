using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{
    [Serializable]
    public class FloatingData
    {
        [field: SerializeField][field: Range(0f, 1f)] public float StepHeightPercentage { get; private set; } = 0.25f;
        [field: SerializeField] public float FloatRayLength { get; private set; } = 2f;
		[field: SerializeField] public float StepHeightMultiplier { get; private set; } = 10f;
		[field: SerializeField] public float CheckWallFWDRayLength { get; private set; } = 0.4f;
	}
}
