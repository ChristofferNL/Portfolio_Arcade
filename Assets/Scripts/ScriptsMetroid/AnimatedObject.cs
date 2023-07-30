using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all objects that will be animated (player, enemies etc)
/// </summary>
public abstract class AnimatedObject : MonoBehaviour
{
    public abstract Animator Animator { get; set; }
    public string CurrentState { get; set; }
    public abstract SpriteRenderer SpriteRenderer { get; set; }

    /// <summary>
    /// Objectstates keeps track of the objects current state and has to be updated accordingly when the conditions for the state is met
    /// </summary>
    public enum ObjectStates
    {
        Idle, Walking, Running, InAir, Shooting, CanWalljump, RunningShooting, InAirShooting, WalkingShooting, TakingDamage, Death
    }
    public abstract ObjectStates ActiveState { get; set; }

    /// <summary>
    /// All classes needs to implement its own dictionary with a string value that matches the different kinds of animations available to the object and match them with the corresponding state
    /// </summary>
    public abstract Dictionary<ObjectStates, string> AnimationValuePairs { get; set; }
    /// <summary>
    /// Plays a animation depending on the current ActiveState and the animation connected with the same key in the AnimationValuePairs dictionary
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeAnimationState(string newState) //Changes the current animation of the object
    {
        //stop the same animation from interrupting itself
        if (CurrentState == newState)
        {
            return;
        }

        Animator.Play(newState);
        CurrentState = newState;
    }
    /// <summary>
    /// Gets the current state from the ActiveState and sends it to the ChangeAnimationState()
    /// </summary>
    public void CheckAnimationState() //Call this method in Update to know what animation to run and set it by calling ChangeAnimationState()
    {
 
        ChangeAnimationState(AnimationValuePairs[ActiveState]);

    }
}
