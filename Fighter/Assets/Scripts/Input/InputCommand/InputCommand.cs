using System.Collections.Generic;
using UnityEngine;

public abstract class InputCommand : ScriptableObject
{
    [Header("Command Properties")]
    public string CommandName;
    public MoveCategory Category;
    
    [Header("Priority & Buffer")]
    [Tooltip("Higher priority = checked first. Supers: 100, Specials: 50, Command Normals: 25, Normals: 10, Movement: 1")]
    public int Priority = 10;
    
    [Tooltip("Should executing this command clear the input buffer?")]
    public bool ClearsBuffer = false;
    
    [Header("Timing")]
    [Tooltip("Max frames ago the input can be (for buffering)")]
    public int BufferWindow = 6;
    
    // Abstract method each command type implements
    public abstract bool CheckInput(PlayerInputBuffer buffer, int facingDirection);
    
    // Virtual method for custom validation
    public virtual bool IsAvailable(State currentState)
    {
        return true;
    }
    
    // Display the command notation
    public abstract string GetNotation();
}