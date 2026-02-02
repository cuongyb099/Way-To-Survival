using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blazer : MonoBehaviour
{
    [Header("Cấu hình")]
    public GameObject firePrefab;      // Prefab bãi lửa
    public int poolSize = 80;           // Số lượng bãi lửa được tạo sẵn
    public float rayDistance = 50f;    // Tầm bắn của ray
    public LayerMask terrainMask;      // Layer terrain
    public float minDelay = 0.1f;      // Delay nhỏ nhất
    public float maxDelay = 0.3f;
    public float firesPerShot = 20;      // Delay lớn nhất

    private List<GameObject> firePool; // Danh sách các bãi lửa
    private int currentIndex = 0;      // Dùng để luân phiên tái sử dụng
    private bool canShoot = true;      // Kiểm tra cooldown
    public LaserTurretAtk LaserTurretAtk;

    void Awake()
    {

    }

    void Start()
    {
        // Khởi tạo pool
        firePool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject fire = Instantiate(firePrefab);
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
    }

    void ShootRay()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, terrainMask))
        {
            // Lấy object trong pool
            GameObject fire = firePool[currentIndex];
            currentIndex = (currentIndex + 1) % poolSize;

            // Đặt vị trí và bật lại
            fire.transform.position = hit.point;
            fire.transform.rotation = Quaternion.LookRotation(hit.normal);
            fire.SetActive(false);
            fire.SetActive(true);

            // Bắt đầu cooldown ngẫu nhiên
            float delay = Random.Range(minDelay, maxDelay);
            StartCoroutine(CooldownRoutine(delay));
        }
    }

    System.Collections.IEnumerator CooldownRoutine(float delay)
    {
        canShoot = false;
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayDistance);
    }
}