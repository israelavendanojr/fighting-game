using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterDashData", menuName = "Character/Dash Data")]
public class CharacterDashData : CharacterStateData
{
    public int Direction = 1; 
    public float DashSpeed;
}