using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Input Commands/Motion (Special/Super)", fileName = "MOT_")]
public class MotionCommand : InputCommand
{
    [Header("Motion Sequence (Numpad Notation)")]
    [Tooltip("Example: 2,3,6 = quarter circle forward (236)")]
    public List<BufferInput> Motion = new List<BufferInput>();
    
    [Header("Button")]
    public BufferInput Button = BufferInput.Light;
    
    [Header("Motion Timing")]
    [Tooltip("Max frames for entire motion")]
    public int MotionWindow = 18;
    
    [Tooltip("How close must final input and button be? (frames)")]
    public int ButtonWindow = 6;
    
    [Header("Leniency")]
    [Tooltip("Allow skipping inputs? Good for lenient 236 (can skip 3)")]
    public bool AllowLenientInputs = true;
    
    public override bool CheckInput(PlayerInputBuffer buffer, int facingDirection)
    {
        // Check if button was pressed
        if (!buffer.WasInputPressed(Button, ButtonWindow))
            return false;
        
        // Convert motion to absolute directions
        List<BufferInput> absoluteMotion = new List<BufferInput>();
        foreach (var input in Motion)
        {
            absoluteMotion.Add(NumpadHelper.ConvertRelativeToAbsolute(input, facingDirection));
        }
        
        // Check if motion was performed
        return CheckMotionInBuffer(buffer, absoluteMotion);
    }
    
    private bool CheckMotionInBuffer(PlayerInputBuffer buffer, List<BufferInput> motion)
    {
        if (motion.Count == 0) return false;
        
        var history = buffer.GetInputHistory();
        int currentFrame = FrameCounter.CurrentFrame;
        int motionIndex = 0;
        
        for (int i = 0; i < history.Count; i++)
        {
            // Check if we're within the motion window
            if (currentFrame - history[i].FramePressed > MotionWindow)
                continue;
            
            // Check if this input matches
            if (history[i].Input == motion[motionIndex])
            {
                motionIndex++;
                
                if (motionIndex >= motion.Count)
                    return true; // Completed!
            }
            else if (!AllowLenientInputs)
            {
                // Strict mode: reset on wrong input
                motionIndex = 0;
            }
        }
        
        return false;
    }
    
    public override string GetNotation()
    {
        string notation = "";
        foreach (var input in Motion)
        {
            notation += ((int)input).ToString();
        }
        notation += Button.ToString()[0];
        return notation;
    }
}