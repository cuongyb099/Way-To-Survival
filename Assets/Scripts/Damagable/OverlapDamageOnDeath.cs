namespace DefaultNamespace.Constructor.Drum
{
    public class OverlapDamageOnDeath : OverlapDamageSender
    {
        protected override void Awake()
        {
            base.Awake();
            if (TryGetComponent(out IDamagable damagable))
            {
                damagable.OnDeath += DealDamage;
            }
        }
    }
}