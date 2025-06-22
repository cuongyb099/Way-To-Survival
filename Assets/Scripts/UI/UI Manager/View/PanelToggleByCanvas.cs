using UnityEngine;

[DefaultExecutionOrder(-1000)]
[RequireComponent(typeof(CanvasGroup))]
public abstract class PanelToggleByCanvas : PanelToggle
{
    protected CanvasGroup canvasGroup;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }

    public override void Hide()
    {
        base.Hide();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public override void Show()
    {
        base.Show();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    
    protected virtual void OnDrawGizmosSelected()
    {
        if(Application.isPlaying) return;
        
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup.alpha > 0.95f)
        {
            this.IsVisible = true; 
            canvasGroup.blocksRaycasts = true;
            return;
        }
     
        this.IsVisible = false;
        canvasGroup.blocksRaycasts = false;
    }
}