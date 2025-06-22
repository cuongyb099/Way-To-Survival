using System.Collections.Generic;
using UnityEngine;

public class ColliderDetectionCtrl : MonoBehaviour
{
    private Dictionary<int, ColliderDetection> _detectionDict;

    private void Awake()
    {
        LoadComponent();
    }

    private void LoadComponent()
    {
        if(_detectionDict != null) return;
        _detectionDict = new Dictionary<int, ColliderDetection>();
        foreach (ColliderDetection detection in transform.parent.GetComponentsInChildren<ColliderDetection>())
        {
            _detectionDict.Add(detection.DetectionName.GetHashCode(), detection);
        }
    }

    public ColliderDetection GetDetection(int hashCode)
    {
        LoadComponent();
        return _detectionDict.GetValueOrDefault(hashCode);
    }

    public ColliderDetection GetDetection(string detectionName)
    {
        return GetDetection(detectionName.GetHashCode());
    }
}