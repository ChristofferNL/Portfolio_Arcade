using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimatedObjectGG : MonoBehaviour
{
    public abstract Animator Animator { get; set; }
    public AnimationClip CurrentState { get; set; }

    /// <summary>
    /// Objectstates keeps track of the objects current state and has to be updated accordingly when the conditions for the state is met
    /// </summary>
    public enum ObjectStates
    {
        Idle, Walking, KnockedBack, Spellcasting, TakingDamage, Death, AttackOne, AttackTwo, AttackThree, Dashing, LimitBreak
    }
    public ObjectStates ActiveState { get; set; }

    /// <summary>
    /// All classes needs to implement its own dictionary with a string value that matches the different kinds of animations available to the object and match them with the corresponding state
    /// </summary>
    public abstract Dictionary<ObjectStates, AnimationClip> AnimationValuePairs { get; set; }
    /// <summary>
    /// Plays a animation depending on the current ActiveState and the animation connected with the same key in the AnimationValuePairs dictionary
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeAnimationState(AnimationClip newState) //Changes the current animation of the object
    {
        if (newState == null)
        {
            //Debug.Log("No animationclip assigned");
            return;
        }
        //stop the same animation from interrupting itself
        if (CurrentState == newState)
        {
            return;
        }
        //Debug.Log(newState);
        Animator.Play(newState.name);
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
