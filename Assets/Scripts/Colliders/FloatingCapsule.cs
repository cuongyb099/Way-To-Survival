using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResilientCore
{
    [Serializable]
    public class FloatingCapsule: MonoBehaviour
    {
		[field: SerializeField] public CapsuleColliderData CapsuleColliderData { get; private set; }
		[field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
        [field: SerializeField] public FloatingData FloatingData { get; private set; }

        public void Awake()
        {
			CapsuleColliderData.Initialize();
			CalculateCapsuleColliderDimention(FloatingData.StepHeightPercentage);
		}

        public void CalculateCapsuleColliderDimention(float percentage)
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);

            SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - percentage));
           
            RecalculateColliderCenter();

            float halfHeight = CapsuleColliderData.Collider.height / 2f;

            if (halfHeight < CapsuleColliderData.Collider.radius)
            {
                SetCapsuleColliderRadius(halfHeight);
            }
            CapsuleColliderData.UpdateColliderData();
        }

        private void RecalculateColliderCenter()
        {
            float halfHeighDiffence = (DefaultColliderData.Height - CapsuleColliderData.Collider.height) / 2f;
            CapsuleColliderData.Collider.center = new Vector3(0, DefaultColliderData.CenterY + halfHeighDiffence, 0);
        }

        private void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.Collider.height = height;
        }
        private void SetCapsuleColliderRadius(float radius)
        {
            CapsuleColliderData.Collider.radius = radius;
        }
    }
}
