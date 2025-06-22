using UnityEngine.Events;

public abstract class PanelToggle : PanelBase
{
    protected UIManager uiManager => UIManager.Instance;

    public UnityEvent OnPanelHide;
    public UnityEvent OnPanelShow;
    
    public override void Hide()
    {
        IsVisible = false;
        uiManager.RemoveFromHitory(this);
        OnPanelHide?.Invoke();       
    }

    public override void Show()
    {
        IsVisible = true;
        uiManager.AddToHitory(this);
        OnPanelShow?.Invoke();       
    }
}