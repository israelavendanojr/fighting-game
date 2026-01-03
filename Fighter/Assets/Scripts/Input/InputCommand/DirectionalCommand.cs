using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Commands/Directional", fileName = "DIR_")]
public class DirectionalCommand : InputCommand
{
    [Header("Direction (Numpad Notation)")]
    [Tooltip("Use numpad: 1=DownBack, 2=Down, 3=DownForward, 4=Back, 5=Neutral, 6=Forward, 7=UpBack, 8=Up, 9=UpForward")]
    public BufferInput Direction = BufferInput.Neutral;
    
    [Tooltip("Must the direction be held (vs just pressed)?")]
    public bool MustBeHeld = true;
    
    public override bool CheckInput(PlayerInputBuffer buffer, int facingDirection)
    {
        // Convert to absolute direction based on facing
        BufferInput absoluteDirection = NumpadHelper.ConvertRelativeToAbsolute(Direction, facingDirection);
        
        if (MustBeHeld)
        {
            return buffer.IsInputHeld(absoluteDirection);
        }
        else
        {
            return buffer.WasInputPressed(absoluteDirection, BufferWindow);
        }
    }
    
    public override string GetNotation()
    {
        return ((int)Direction).ToString();
    }
}