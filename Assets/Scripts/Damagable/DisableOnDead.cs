using System;
using UnityEngine;

namespace DefaultNamespace.Constructor.Drum
{
    public class DisableOnDead : MonoBehaviour
    {
        private void Awake()
        {
            if (TryGetComponent(out IDamagable damagable))
            {
                damagable.OnDeath += () =>
                {
                    this.gameObject.SetActive(false);
                };
            };
        }
    }
}