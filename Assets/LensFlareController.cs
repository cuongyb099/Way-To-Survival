using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LensFlareController : MonoBehaviour
{
    public AnimationCurve intensityCurve;
    public float duration = 2f;

    private float timer;
    [SerializeField] public LensFlareComponentSRP LensFlare;

    private float baseScale;

    void Awake()
    {
        LensFlare = GetComponent<LensFlareComponentSRP>();
        timer = 0f;
        // Lưu lại scale gốc
        baseScale = LensFlare.scale;
        LensFlare.enabled = false;
    }

    void Update()
    {
        LensFlare.enabled = true;
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        float value = intensityCurve.Evaluate(t);
        // Nhân với baseScale, KHÔNG dùng lại giá trị đã bị modify
        LensFlare.scale = baseScale * value;

        if (timer >= duration)
            enabled = false;
    }
}