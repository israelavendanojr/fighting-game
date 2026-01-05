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

    [Header("Attack")]
    // idea is that all entries will plan after another to allow multihit attacks
    public AttackData[] attacks;
    
    [Header("Movement")]
    public Vector2 initialVelocity;
    public AnimationCurve velocityOverTime;
    
    [Header("Cancel Options")]
    public bool specialCancelable = true;
    public bool superCancelable = true;
}

[System.Serializable]
public class AttackData
{
    [Header("Frame Data")]
    public int startup;
    public int active;
    public int recovery;
    public int hitStun; 
    
    [Header("Boxes")]
    public List<Vector2> hitboxes;
    public List<Vector2> hurtboxes;
}