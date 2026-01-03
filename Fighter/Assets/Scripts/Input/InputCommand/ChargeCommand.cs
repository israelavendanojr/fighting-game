using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Commands/Charge", fileName = "CHG_")]
public class ChargeCommand : InputCommand
{
    [Header("Charge Direction")]
    [Tooltip("Direction to hold (typically 4=Back or 2=Down)")]
    public BufferInput ChargeDirection = BufferInput.Back;
    
    [Tooltip("Frames required to hold charge")]
    public int RequiredChargeFrames = 40;
    
    [Header("Release Direction")]
    [Tooltip("Direction to release (typically 6=Forward or 8=Up)")]
    public BufferInput ReleaseDirection = BufferInput.Forward;
    
    [Header("Button")]
    public BufferInput Button = BufferInput.Light;
    
    [Tooltip("How close must release and button be? (frames)")]
    public int ReleaseWindow = 6;
    
    public override bool CheckInput(PlayerInputBuffer buffer, int facingDirection)
    {
        // Check button press
        if (!buffer.WasInputPressed(Button, ReleaseWindow))
            return false;
        
        // Convert to absolute directions
        BufferInput absoluteCharge = NumpadHelper.ConvertRelativeToAbsolute(ChargeDirection, facingDirection);
        BufferInput absoluteRelease = NumpadHelper.ConvertRelativeToAbsolute(ReleaseDirection, facingDirection);
        
        // Check if we held charge long enough
        if (buffer.GetHeldFrames(absoluteCharge) < RequiredChargeFrames)
            return false;
        
        // Check if we released recently
        return buffer.WasInputPressed(absoluteRelease, ReleaseWindow);
    }
    
    public override string GetNotation()
    {
        return $"[{(int)ChargeDirection}]{(int)ReleaseDirection}{Button.ToString()[0]}";
    }
}