using UnityEngine;

public class IndicatorBuilding : MonoBehaviour
{
    [SerializeField] protected Renderer[] renderers;

    public static readonly int SurfaceID = Shader.PropertyToID("_Preset");
    public static readonly int ZWrite = Shader.PropertyToID("_ZWrite");
    public static readonly int SourceBlend = Shader.PropertyToID("_SourceBlend");
    public static readonly int DestBlend = Shader.PropertyToID("_DestBlend");
    
    private void Reset()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }
    
    public void SetIndicator(Color color)
    {
        foreach (var render in renderers)
        {
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            foreach (var mat in render.materials)
            {
                mat.SetFloat(SurfaceID, 1);
                mat.SetFloat(SourceBlend, 5); // SrcAlpha
                mat.SetFloat(DestBlend, 10); // OneMinusSrcAlpha
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                mat.color = color;
            }
        }
    }

    public void ReturnDefault()
    {
        foreach (var render in renderers)
        {
            render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            foreach (var mat in render.materials)
            {
                mat.SetFloat(SurfaceID, 0);
                mat.SetFloat(SourceBlend, 1);    // One
                mat.SetFloat(DestBlend, 0);      // Zero
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry; 
                mat.color = Color.white;
            }
        }
    }
}