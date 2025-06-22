using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Update = UnityEngine.PlayerLoop.Update;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] [Range(0f, 100f)] private float defaultDistance = 6f;
    [SerializeField] [Range(0f, 100f)] private float maxDistance = 8f;
    [SerializeField] [Range(0f, 100f)] private float minDistance = 2f;

    [SerializeField] [Range(0f, 100f)] private float smoothing = 4f;
    [SerializeField] [Range(0f, 2f)] private float zoomSensitivity = 1f;

    [SerializeField] private float currentTargetDistance;

    private CinemachineInputProvider inputProvider;
    private CinemachineTransposer transposer;

    private void Start()
    {
        transposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineTransposer>();
        inputProvider = GetComponent<CinemachineInputProvider>();
    }

    // Update is called once per frame
    private void Update()
    {
        Zoom();
    }
  //   private void Zoom()
  //   {
  //       float zoomValue = -inputProvider.GetAxisValue(2) * zoomSensitivity;
  //       currentTargetDistance = Mathf.Clamp(currentTargetDistance + zoomValue, minDistance, maxDistance);
  //
  //       float currentDistance = transposer.m_FollowOffset.magnitude;
  //       if (currentTargetDistance == currentDistance)
  //       {
  //           return;
  //       }
  //       float learpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);
  //
		// transposer.m_FollowOffset = transposer.m_FollowOffset.normalized * learpedZoomValue;
  //   }
    public void Zoom()
    {
        float currentDistance = transposer.m_FollowOffset.magnitude;
        if (currentTargetDistance == currentDistance)
        {
            return;
        }
        float learpedZoomValue = Mathf.Lerp(currentDistance, currentTargetDistance, smoothing * Time.deltaTime);

        transposer.m_FollowOffset = transposer.m_FollowOffset.normalized * learpedZoomValue;
    }

    public void SetZoom(float zoom)
    {
        currentTargetDistance = Mathf.Clamp(zoom, minDistance, maxDistance);
    }
}
