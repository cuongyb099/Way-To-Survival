
using UnityEngine;

namespace DefaultNamespace.Projectile
{
    public interface IProjectile
    {
        public void Init(Vector3 direction, float force);
    }
}