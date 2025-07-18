using System.Collections.Generic;
using KatLib.Serialized_Type;
using UnityEngine;

public class ColliderDetectionCtrl : MonoBehaviour
{
    private Dictionary<string, ColliderDetection> _detectionDict;

    private void Awake()
    {
        LoadComponent();
    }

    private void LoadComponent()
    {
        if(_detectionDict != null) return;
        _detectionDict = new Dictionary<string, ColliderDetection>();
        foreach (ColliderDetection detection in transform.parent.GetComponentsInChildren<ColliderDetection>())
        {
            _detectionDict.Add(detection.DetectionName.Value, detection);
        }
    }
    
    public ColliderDetection GetDetection(StringSO detectionName)
    {
        return GetDetection(detectionName.Value);
    }

    public ColliderDetection GetDetection(string detectionName)
    {
        return _detectionDict.GetValueOrDefault(detectionName);
    }
}