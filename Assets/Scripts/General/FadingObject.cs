using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FadingObject : MonoBehaviour
{
    public static readonly int SurfaceID = Shader.PropertyToID("_Preset");
    public static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
    public static readonly int SourceBlend = Shader.PropertyToID("_SourceBlend");
    public static readonly int DestBlend = Shader.PropertyToID("_DestBlend");
    
    public List<Renderer> Renderers = new ();
    protected List<Material> fadeMats = new List<Material>();
    protected Tween fadeTween;
    protected float currentAlpha = 1;
    
    protected virtual void Reset()
    {
        LoadComponents();
    }

    private void Awake()
    {
        LoadComponents();
    
        foreach (var render in Renderers)
        {
            foreach (var mat in render.materials)
            {
                fadeMats.Add(mat);
            }
        }
    }

    protected virtual void LoadComponents()
    {
        if (Renderers == null || Renderers.Count == 0)
        {
            Renderers.AddRange(GetComponentsInChildren<Renderer>());
        }
    }

    public virtual void FadeOut(float duration)
    {
        if (fadeTween != null && fadeTween.IsActive())
        {
            fadeTween.Kill();       
        }

        for (var i = 0; i < fadeMats.Count; i++)
        {
            Material mat = fadeMats[i];
            mat.SetFloat(SurfaceID, 1);
            mat.SetFloat(SourceBlend, 5); // SrcAlpha
            mat.SetFloat(DestBlend, 10); // OneMinusSrcAlpha
            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent - i;
        }

        fadeTween = DOVirtual.Float(1f, 0.33f, duration, (value) =>
        {
            currentAlpha = value;
            foreach (var mat in fadeMats)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, value);
            }
        }).SetEase(Ease.Linear);
    }

    public virtual void FadeIn(float duration)
    {
        if (fadeTween != null && fadeTween.IsActive())
        {
            fadeTween.Kill();       
        }

        fadeTween = DOVirtual.Float(currentAlpha, 1f, duration, (value) =>
        {
            foreach (var mat in fadeMats)
            {
                mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, value);
            }
        }).OnComplete(() =>
        {
            foreach (var mat in fadeMats)
            {
                mat.SetFloat(SurfaceID, 0);
                mat.SetFloat(SourceBlend, 1);    // One
                mat.SetFloat(DestBlend, 0);      // Zero
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry; 
            }
        }).SetEase(Ease.Linear);
    }
}