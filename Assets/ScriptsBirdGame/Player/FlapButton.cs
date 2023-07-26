using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Used if the game is played on a mobile device, else the new Unity input system is used 
/// </summary>
public class FlapButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    bool buttonPressed;
    bool usingKeyboard;

    private void Start()
    {
        usingKeyboard = FindObjectOfType<InputManagerBirdGame>().UsingKeyboard;
    }

    private void Update()
    {
        if (!usingKeyboard)
        {
            EventManager.Instance.CallFlapEvent(buttonPressed);
        }  
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
