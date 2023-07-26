using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Singleton eventmanager that handles player movement, pick up of items and the call to finish a round of play
/// </summary>
public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent<float> EventMoveInput;
    public UnityEvent<bool> EventFlapInput;
    public UnityEvent EventItemPickUp;
    public UnityEvent EventFinishRound;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void CallFinishRoundEvent()
    {
        EventFinishRound.Invoke();
    }

    public void CallPickUpItemEvent()
    {
        EventItemPickUp.Invoke();
    }

    public void CallMoveEvent(float input)
    {
        EventMoveInput.Invoke(input);
    }

    public void CallFlapEvent(bool isPressed)
    {
        EventFlapInput.Invoke(isPressed);
    }
}

