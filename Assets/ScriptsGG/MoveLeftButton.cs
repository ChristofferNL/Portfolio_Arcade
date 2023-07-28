using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveLeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool buttonPressed;

    private void Update()
    {
        if (buttonPressed)
        {
            EventManagerGG.Instance.MoveInputEvent(-1.00f);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        EventManagerGG.Instance.MoveInputEvent(0f);
    }
}
