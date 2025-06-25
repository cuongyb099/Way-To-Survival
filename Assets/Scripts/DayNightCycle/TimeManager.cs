using DG.Tweening;
using Tech.Singleton;
using UnityEngine;

public enum TimeOfTheDay
{
    MidNight,
    EarlyMorning,
    Morning,
    Noon,
    Afternoon,
    Evening,
}

public class TimeManager : Singleton<TimeManager>
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    [field: SerializeField] public TimeOfTheDay CurrentTOD { get; private set; }
    [field: SerializeField, Range(0, 24)] public float TimeOfDay { get; private set; }
    private float LastTimeOfDay;

    private void Start()
    {
        ChangeTimeOfDayName();
    }
    
    [field:SerializeField] public float TransitionDuration { get; private set; }
    private Tween tween;

    [ContextMenu("Change Time of Day")]
    public void AdvanceTimeOfDay()
    {
        if (!Preset)
            return;
        
        tween.Complete();
        tween = DOVirtual.Float(TimeOfDay, TimeOfDay + 4, TransitionDuration, v =>
        {
            TimeOfDay = v % 24;
            UpdateLighting(TimeOfDay / 24f);
        }).SetEase(Ease.Linear);
        tween.onComplete = () => ChangeTimeOfDayName();
    }

    private void ChangeTimeOfDayName()
    {
        CurrentTOD = (TimeOfTheDay)((int)(TimeOfDay/4) %6);
        GameEvent.OnChangeTimeOfDay?.Invoke(CurrentTOD);
    }
    
    private void UpdateLighting(float timePercent)
    {
        //Set ambient and fog
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        RenderSettings.ambientIntensity = Preset.LightingIntensity.Evaluate(timePercent);

        //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }

    }

    //Try to find a directional light to use if we haven't set one
    // private void OnValidate()
    // {
    //     if (DirectionalLight != null)
    //         return;
    //
    //     //Search for lighting tab sun
    //     if (RenderSettings.sun != null)
    //     {
    //         DirectionalLight = RenderSettings.sun;
    //     }
    //     //Search scene for light that fits criteria (directional)
    //     else
    //     {
    //         Light[] lights = GameObject.FindObjectsOfType<Light>();
    //         foreach (Light light in lights)
    //         {
    //             if (light.type == LightType.Directional)
    //             {
    //                 DirectionalLight = light;
    //                 return;
    //             }
    //         }
    //     }
    // }
}