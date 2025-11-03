using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurretAtk : MonoBehaviour
{
    [Header("Cấu hình")]
    public Animator animator;                     // Animator có chứa clip tilt
    public string animationName = "LaserTurret";  // Tên clip tilt
    public bool repeat = true;                    // Có lặp lại hay không
    [Tooltip("Độ trễ giữa các lần tấn công (giây). Giá trị càng nhỏ => tốc độ bắn càng cao.")]
    public float ATKDelay = 2f;                   // Độ trễ giữa các lần bắn

    [Header("Flare")]
    public GameObject flarePrefab;                // Prefab của Flare (GameObject)
    public Transform flareSpawnPoint;             // Vị trí nguồn bắn (đầu phát laser)
    public int flarePoolSize = 10;                // Số lượng flare trong pool
    public float flareLifetime = 0.5f;            // Thời gian flare tồn tại trước khi tắt

    private Renderer[] renderers;
    public bool isFiring = false;
    private float baseAnimLength = 1f;

    // Pool chứa flare
    private Queue<GameObject> flarePool = new Queue<GameObject>();

    void Start()
    {
        flareSpawnPoint.position = this.transform.position;
        renderers = GetComponentsInChildren<Renderer>();

        if (animator == null)
            animator = GetComponent<Animator>();

        baseAnimLength = GetAnimationLength(animationName);

        InitializeFlarePool();

        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        if (baseAnimLength > 0)
        {
            animator.speed = 1 / ATKDelay;
        }
    }

    private IEnumerator FireRoutine()
    {
        isFiring = true;

        do
        {
            // Phát animation
            animator.Play(animationName, 0, 0f);

            // Spawn flare tại đầu phát
            SpawnFlare();

            // Thời gian chạy animation (đã chia theo tốc độ)
            float currentAnimLength = baseAnimLength / animator.speed;
            yield return new WaitForSeconds(currentAnimLength);

            // Độ trễ còn lại (nếu có)
            float remainingDelay = Mathf.Max(ATKDelay - currentAnimLength, 0f);
            yield return new WaitForSeconds(remainingDelay);

        } while (repeat);

        isFiring = false;
    }

    private void InitializeFlarePool()
    {
        if (flarePrefab == null) return;

        for (int i = 0; i < flarePoolSize; i++)
        {
            GameObject flare = Instantiate(flarePrefab);
            flare.SetActive(false);
            flarePool.Enqueue(flare);
        }
    }

    private void SpawnFlare()
    {
        if (flarePrefab == null || flareSpawnPoint == null || flarePool.Count == 0)
            return;

        GameObject flare = flarePool.Dequeue();

        flare.transform.SetPositionAndRotation(flareSpawnPoint.position, flareSpawnPoint.rotation);
        flare.SetActive(true);

        // Kích hoạt lại particle/VFX (nếu có component)
        var ps = flare.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Clear(true);
            ps.Play(true);
        }

        // Tự động tắt flare sau thời gian quy định
        StartCoroutine(DeactivateFlareAfter(flare, flareLifetime));

        // Đưa lại flare vào pool
        flarePool.Enqueue(flare);
    }

    private IEnumerator DeactivateFlareAfter(GameObject flare, float time)
    {
        yield return new WaitForSeconds(time);
        if (flare != null)
            flare.SetActive(false);
    }

    private void SetVisible(bool visible)
    {
        if (renderers == null) return;
        foreach (var r in renderers)
            r.enabled = visible;
    }

    private float GetAnimationLength(string clipName)
    {
        if (animator == null || animator.runtimeAnimatorController == null)
            return 0f;

        foreach (var clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        return 0f;
    }
}
