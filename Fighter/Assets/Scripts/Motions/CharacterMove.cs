using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterMove", menuName = "Character/Character Move")]

public class CharacterMove : ScriptableObject
{
    [Header("Input")]
    public CommandInput commandInput;
    
    [Header("Animation")]
    public AnimationClip animationClip;
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    
    [Header("Combat")]
    public List<Vector2> hitboxes;
    public List<Vector2> hurtboxes;
    
    [Header("Movement")]
    public Vector2 initialVelocity;
    public AnimationCurve velocityOverTime;
    
    [Header("Cancel Options")]
    public bool specialCancelable = true;
    public bool superCancelable = true;
}
