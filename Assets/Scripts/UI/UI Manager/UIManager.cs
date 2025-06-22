using System.Collections.Generic;
using System;
using Cysharp.Threading.Tasks;
using Tech.Logger;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIManager : Tech.Singleton.Singleton<UIManager>
{
   [SerializeField] private List<PanelBase> _panelsHistory = new();
    private CanvasGroup _canvasGroup;
    private Dictionary<string, PanelBase> _panelDictionary = new();
    
    protected override void Awake()
    {
        base.Awake();
        
        _canvasGroup = GetComponent<CanvasGroup>();
        _panelsHistory.Clear();
        foreach (var panel in GetComponentsInChildren<PanelBase>())
        {
            _panelDictionary.Add(panel.name, panel);
         
            if (!panel.IsVisible) continue;
            
            _panelsHistory.Add(panel);
        }
    }

    public void EnableUI()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }
    
    public void DisableUI()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
    
    public void AddToHitory(PanelToggle panel)
    {
        if(_panelsHistory.Contains(panel)) return;
        
        _panelsHistory.Add(panel);
    }

    public void RemoveFromHitory(PanelToggle panel)
    {
        _panelsHistory.Remove(panel);
    }
    
    public async UniTask<PanelToggleByCanvas> CreatePanelAsync(string panelName, Transform parent = null, Action<PanelToggleByCanvas> onComplete = null)
    {
        if (_panelDictionary.ContainsKey(panelName)) return null;

        var go = await AddressablesManager.Instance.InstantiateAsync(panelName, parent? parent: transform, false);
        go.name = panelName;
        
        if (!go.TryGetComponent(out PanelToggleByCanvas panel))
        {
            LogCommon.LogError(go.name + "No Has Panel Component");
            return null;
        }
        onComplete?.Invoke(panel);

        _panelDictionary.Add(panelName, panel);
        return panel;
    }

    public void RemovePanel(string panelName)
    {
        if(!_panelDictionary.TryGetValue(panelName, out var panel)) return;
        
        _panelsHistory.Remove(panel);
        _panelDictionary.Remove(panelName);
        
        AddressablesManager.Instance.ReleaseInstance(panelName);
    }

    public T GetPanel<T>(string panelName) where T : PanelBase
    {
        return (T)_panelDictionary.GetValueOrDefault(panelName);
    }

    public T GetFirstPanelOfType<T>() where T : PanelBase
    {
        foreach (var panel in _panelDictionary.Values)
        {
            if (panel is T tPanel) return tPanel;
        }

        return null;
    }
    
    public void ShowPanel(string panelName)
    {
        if (!_panelDictionary.TryGetValue(panelName, out var panel) 
            || _panelsHistory.Contains(panel)) return;
        
        panel.Show();
    }

    public void HidePanel(string panelName)
    {
        if (!_panelDictionary.TryGetValue(panelName, out var panel)) return;
        
        panel.Hide();
    }

    public void HideCurrentPanel()
    {
        if(_panelsHistory.Count == 0) return;
        
        _panelsHistory[^1].Hide();
    }
}
