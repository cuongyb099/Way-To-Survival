using UnityEngine;
public interface IFlyable 
{
    public void ApplyFly(Vector3 direction, float force);
    public void SetLayerMask(LayerMask includeLayer = default, LayerMask excludeLayer = default);
    public void Stop();
}