using System.Collections.Generic;
using UnityEngine;
public static class NumpadHelper
{
    // Convert numpad int to BufferInput (1-9 to DownBack-UpForward)
    public static BufferInput NumpadToInput(int numpad)
    {
        return (BufferInput)numpad;
    }
    
    // Convert relative numpad to absolute based on facing
    // facingDirection: 1 = right (forward is 6), -1 = left (forward is 4)
    public static BufferInput ConvertRelativeToAbsolute(BufferInput input, int facingDirection)
    {
        if (facingDirection > 0) return input; // Facing right, no conversion needed
        
        // Facing left, flip horizontally
        switch (input)
        {
            case BufferInput.Forward: return BufferInput.Back;          // 6 → 4
            case BufferInput.Back: return BufferInput.Forward;          // 4 → 6
            case BufferInput.DownForward: return BufferInput.DownBack;  // 3 → 1
            case BufferInput.DownBack: return BufferInput.DownForward;  // 1 → 3
            case BufferInput.UpForward: return BufferInput.UpBack;      // 9 → 7
            case BufferInput.UpBack: return BufferInput.UpForward;      // 7 → 9
            default: return input; // 2, 5, 8 stay the same
        }
    }
    
    // Check if buffer input is a direction
    public static bool IsDirection(BufferInput input)
    {
        return (int)input >= 1 && (int)input <= 9;
    }
    
    // Check if buffer input is a button
    public static bool IsButton(BufferInput input)
    {
        return input == BufferInput.Light || 
               input == BufferInput.Medium || 
               input == BufferInput.Heavy || 
               input == BufferInput.Dash || 
               input == BufferInput.Throw;
    }
    
    // Display notation as string
    public static string GetNotation(List<BufferInput> inputs)
    {
        string result = "";
        foreach (var input in inputs)
        {
            if (IsDirection(input))
            {
                result += ((int)input).ToString();
            }
            else if (IsButton(input))
            {
                // Just first letter for buttons
                result += input.ToString()[0];
            }
        }
        return result;
    }
}