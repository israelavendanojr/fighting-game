using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Commands/Normal", fileName = "NRM_")]
public class NormalCommand : InputCommand
{
    [Header("Button")]
    public BufferInput Button = BufferInput.Light;
    
    [Header("Direction (Optional)")]
    [Tooltip("5=Neutral/Standing, 2=Crouching, 6=Forward, etc.")]
    public BufferInput Direction = BufferInput.Neutral;
    
    [Tooltip("How close must direction and button be? (frames)")]
    public int SimultaneousWindow = 3;
    
    public override bool CheckInput(PlayerInputBuffer buffer, int facingDirection)
    {
        // Check if button was pressed
        if (!buffer.WasInputPressed(Button, BufferWindow))
            return false;
        
        // Convert direction to absolute
        BufferInput absoluteDirection = NumpadHelper.ConvertRelativeToAbsolute(Direction, facingDirection);
        
        // Check if direction is held
        return buffer.IsInputHeld(absoluteDirection);
    }
    
    public override string GetNotation()
    {
        return $"{(int)Direction}{Button.ToString()[0]}";
    }
}