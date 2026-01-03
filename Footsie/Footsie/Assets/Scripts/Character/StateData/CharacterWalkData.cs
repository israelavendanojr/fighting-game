using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterWalkData", menuName = "Character/Walk Data")]
public class CharacterWalkData : CharacterStateData
{
    public int Direction = 1; 
    public float WalkSpeed;
}