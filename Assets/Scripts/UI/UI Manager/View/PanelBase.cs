using UnityEngine;

public abstract class PanelBase : MonoBehaviour
{
    [field: SerializeField] public virtual bool IsVisible { get; protected set; }
    
    public virtual void Hide(){}
    public virtual void Show(){}
}