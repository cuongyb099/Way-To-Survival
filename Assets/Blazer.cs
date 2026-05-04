using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blazer : MonoBehaviour
{
    [Header("Cấu hình")]
    public GameObject firePrefab;
    public int poolSize = 80;
    public float rayDistance = 50f;
    public LayerMask terrainMask;

    [Header("Phân bố bãi lửa")]
    public float minDistanceBetweenFires = 0.9f; // khoảng cách tối thiểu giữa các bãi lửa
    public float maxDistanceBetweenFires = 2f; // khoảng cách tối thiểu giữa các bãi lửa

    public LaserTurretAtk LaserTurretAtk;

    private List<GameObject> firePool;
    private int currentIndex = 0;

    private Vector3 lastSpawnPoint;
    private bool hasLastPoint = false;

    void Start()
    {
        // Khởi tạo pool
        firePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fire = Instantiate(firePrefab, Vector3.zero, Quaternion.identity);
            fire.SetActive(false);
            firePool.Add(fire);
        }
    }

    void Update()
    {
        if (LaserTurretAtk.isFiring)
        {
            ShootRay();
        }
        else
        {
            // reset khi ngừng bắn để lần sau không bị lệch spacing
            hasLastPoint = false;
        }
    }

    void ShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, terrainMask))
        {
            // chỉ spawn nếu đủ khoảng cách
            if (!hasLastPoint || Vector3.Distance(hit.point, lastSpawnPoint) >= minDistanceBetweenFires 
            || Vector3.Distance(hit.point, lastSpawnPoint) <= maxDistanceBetweenFires)
            {
                GameObject fire = firePool[currentIndex];
                currentIndex = (currentIndex + 1) % poolSize;

                fire.transform.position = hit.point;

                fire.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                // restart effect (nếu cần)
                fire.SetActive(false);
                fire.SetActive(true);

                lastSpawnPoint = hit.point;
                hasLastPoint = true;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}