using UnityEngine;

public class PlaceHitBox : MonoBehaviour
{
    [field: SerializeField] public Vector3 Size { get; protected set; }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, this.Size);
    }
}