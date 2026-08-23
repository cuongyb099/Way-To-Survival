using UnityEngine;

public class Blazer : MonoBehaviour
{
    [Header("Raycast")]
    public float rayDistance = 50f;
    public LayerMask terrainMask;

    [Header("Fire Spawn")]
    public string firePoolName = "Fire";

    // Spawn thường nếu không dùng Pool
    public GameObject firePrefab;

    public float minDistanceBetweenFires = 1f;

    public LaserTurretAtk LaserTurretAtk;

    private Vector3 lastSpawnPoint;
    private bool hasLastPoint;

    private void Update()
    {
        if (LaserTurretAtk.isFiring)
        {
            ShootRay();
        }
        else
        {
            hasLastPoint = false;
        }
    }

    private void ShootRay()
    {
        Ray ray = new Ray(
            transform.position,
            transform.forward);

        if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                rayDistance,
                terrainMask))
        {
            return;
        }

        float distance =
            hasLastPoint
            ? Vector3.Distance(hit.point, lastSpawnPoint)
            : float.MaxValue;

        if (!hasLastPoint ||
            distance >= minDistanceBetweenFires)
        {
            Quaternion rotation =
                Quaternion.FromToRotation(
                    Vector3.up,
                    hit.normal);

            SpawnFire(hit.point, rotation);

            lastSpawnPoint = hit.point;
            hasLastPoint = true;
        }
    }

    private void SpawnFire(Vector3 position, Quaternion rotation)
    {
        if (ObjectPoolManager.Instance != null &&
            ObjectPoolManager.Instance.HasPool(firePoolName))
        {
            ObjectPoolManager.Instance.SpawnFromPool(
                firePoolName,
                position,
                rotation);
        }
        else
        {
            GameObject fire = Instantiate(
                firePrefab,
                position,
                rotation);

            Destroy(fire, 5f); // hoặc cùng thời gian tồn tại của hiệu ứng
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            transform.position,
            transform.forward * rayDistance);
    }
}