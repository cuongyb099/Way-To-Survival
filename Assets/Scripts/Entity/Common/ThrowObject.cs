using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ThrowObject : MonoBehaviour, IFlyable
{
    protected Rigidbody rb;
    protected DamageInfo damage;
    public bool IsDone {get; protected set;}
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();        
    }

    protected virtual void OnEnable()
    {
        IsDone = false;
    }

    protected virtual void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

    public virtual void SetLayerMask(LayerMask includeLayer = default, LayerMask excludeLayer = default){
        rb.includeLayers = includeLayer;
        rb.excludeLayers = excludeLayer;
    }

    public virtual void ThrowProjectionMotion(float angle , Vector3 startPoint, Vector3 endPoint, float timeReachEndPoint = 1f){
        float radAngle = angle * Mathf.Deg2Rad;
        var velocity = CalculateInitVelocity(radAngle, startPoint, endPoint, timeReachEndPoint);
        var velocityX = velocity * Mathf.Cos(radAngle);
        var velocityY = velocity * Mathf.Sin(radAngle);
        Vector3 vectorVelocity = new Vector3(0, velocityY, velocityX);
        
        var directionToTarget = endPoint - startPoint;
        var rotation = Quaternion.AngleAxis(angle, Vector3.Cross(directionToTarget, Vector3.up));
        var throwDir = (rotation * Vector3.ProjectOnPlane(directionToTarget, Vector3.up)).normalized * vectorVelocity.magnitude;
        rb.AddForce(throwDir, ForceMode.Impulse);
    }

    protected virtual float CalculateInitVelocity(float radAngle, Vector3 startPoint, Vector3 endPoint, float time){
        //Projection motion
        var vector = endPoint - startPoint;
        vector.y = 0;
        float x = vector.magnitude;
        float y = endPoint.y - startPoint.y;

        //y = (-g / 2.v^2 cosa^2)x^2 + x.tana => V = 
        float velocityInit = x * x * 10f / (2 * Mathf.Cos(radAngle) * Mathf.Cos(radAngle) * (x * Mathf.Tan(radAngle) - y));
        return Mathf.Sqrt(Mathf.Abs(velocityInit));
    }

    public void ApplyFly(Vector3 direction, float force)
    {
        rb.AddForce(direction.normalized * force, ForceMode.Impulse);
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }
}