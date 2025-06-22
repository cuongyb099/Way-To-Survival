using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.InputSystem.EnhancedTouch;
using Unity.Services.Analytics.Internal;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using Tech.Logger;

public class FloatingJoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    [SerializeField]
    GameObject joystick;

    OnScreenStick screenStick;
    RectTransform mainRect;
    RectTransform joystickRect;

    Image[] Images;
    private void Awake()
    {
        screenStick = joystick.GetComponentInChildren<OnScreenStick>();
        mainRect = GetComponent<RectTransform>();
        joystickRect = joystick.GetComponent<RectTransform>();
        Images = joystick.GetComponentsInChildren<Image>();
        HideJoystick();
    }
    public void OnDrag(PointerEventData eventData)
    {
        ExecuteEvents.dragHandler(screenStick, eventData);
    }

    GameObject b;
    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainRect, eventData.pressPosition, eventData.pressEventCamera, out localPosition);
        ShowJoystick();
        joystickRect.localPosition = localPosition;
        ExecuteEvents.pointerDownHandler(screenStick, eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HideJoystick();
        ExecuteEvents.pointerUpHandler(screenStick, eventData);
    }
    public void HideJoystick()
    {
        foreach (var img in Images)
        {
            img.DOFade(0, .1f);
        }
    }
    public void ShowJoystick()
    {
        foreach (var img in Images)
        {
            img.DOFade(1, .1f);
        }
    }
}
