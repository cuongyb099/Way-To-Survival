using UnityEngine;

[RequireComponent(typeof(FadeUI))]
public abstract class FadePanel : PanelToggle
{
    protected const float _fadeDuration = 0.25f;
    protected FadeUI fadeUI;
    
    private void Awake()
    {
        fadeUI = GetComponent<FadeUI>();
        OnAwake();
    }

    protected virtual void OnAwake() { }

    public override void Hide()
    {
        base.Hide();
        fadeUI.FadeOut(_fadeDuration, () =>
        {
            this.IsVisible = false;
        });
    }

    public override void Show()
    {
        base.Show();
        fadeUI.FadeIn(_fadeDuration, () =>
        {
            this.IsVisible = true;
        });
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        if(Application.isPlaying) return;
         
        var canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup.alpha > 0.95f)
        {
            this.IsVisible = true;
            canvasGroup.blocksRaycasts = true;
            return;
        }
     
        this.IsVisible = false;
        canvasGroup.blocksRaycasts = false;
    }
}