using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Commands/Throw", fileName = "THR_")]
public class ThrowCommand : InputCommand
{
    [Header("Throw Input")]
    [Tooltip("Direction for throw (6=Forward Throw, 4=Back Throw, 5=Neutral Throw)")]
    public BufferInput Direction = BufferInput.Forward;
    
    [Tooltip("Throw button is dedicated, or requires Light+Medium?")]
    public bool UseDedicatedButton = true;
    
    [Header("Multi-Button (if not dedicated)")]
    [Tooltip("For games without throw button: press L+M simultaneously")]
    public BufferInput Button1 = BufferInput.Light;
    public BufferInput Button2 = BufferInput.Medium;
    
    [Tooltip("How close must buttons be pressed together? (frames)")]
    public int SimultaneousWindow = 3;
    
    public override bool CheckInput(PlayerInputBuffer buffer, int facingDirection)
    {
        BufferInput absoluteDirection = NumpadHelper.ConvertRelativeToAbsolute(Direction, facingDirection);
        
        // Check direction
        if (!buffer.IsInputHeld(absoluteDirection))
            return false;
        
        // Check button(s)
        if (UseDedicatedButton)
        {
            return buffer.WasInputPressed(BufferInput.Throw, BufferWindow);
        }
        else
        {
            // Check if both buttons pressed within window
            return buffer.WasInputPressed(Button1, SimultaneousWindow) && 
                   buffer.WasInputPressed(Button2, SimultaneousWindow);
        }
    }
    
    public override string GetNotation()
    {
        if (UseDedicatedButton)
            return $"{(int)Direction}T";
        else
            return $"{(int)Direction}{Button1.ToString()[0]}+{Button2.ToString()[0]}";
    }
}