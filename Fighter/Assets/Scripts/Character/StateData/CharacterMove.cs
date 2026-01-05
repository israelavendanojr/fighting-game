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
    
    [Header("Damage")]
    public int damage = 10;
    public Vector2 knockback = new Vector2(5f, 3f);
    
    [Header("Boxes")]
    public BoxData[] hitboxes;
    public BoxData[] hurtboxes;
}

[System.Serializable]
public class BoxData
{
    public Vector2 offset;
    public Vector2 size = new Vector2(1f, 1f);
}