using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterMovement", menuName = "Character/Character Movement")]
public class CharacterMovement : ScriptableObject
{
    [Header("Input")]
    public CommandInput commandInput;
    
    [Header("Movement Properties")]
    public float speed = 5f;
    public AnimationClip animationClip;
    
    [Header("Physics")]
    public bool affectedByGravity = true;
    public float friction = 0.9f;
    
    [Header("Transition")]
    public bool canTransitionToAttacks = true;
    public bool canTransitionToOtherMovement = true;
}