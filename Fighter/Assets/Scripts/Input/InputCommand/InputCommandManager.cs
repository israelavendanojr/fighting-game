using System.Collections.Generic;
using UnityEngine;
public class InputCommandManager : MonoBehaviour
{
    [Header("Command List")]
    public List<InputCommand> Commands = new List<InputCommand>();
    
    [Header("Settings")]
    [Tooltip("Character's facing: 1 = right, -1 = left")]
    public int FacingDirection = 1;
    
    private PlayerInputHandler _inputHandler;
    private List<InputCommand> _sortedCommands;
    
    private void Awake()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
        SortCommands();
    }
    
    private void SortCommands()
    {
        _sortedCommands = new List<InputCommand>(Commands);
        _sortedCommands.Sort((a, b) => b.Priority.CompareTo(a.Priority));
    }
    
    public InputCommand CheckCommands(State currentState = null)
    {
        foreach (var command in _sortedCommands)
        {
            if (currentState != null && !command.IsAvailable(currentState))
                continue;
            
            if (command.CheckInput(_inputHandler.InputBuffer, FacingDirection))
            {
                return command;
            }
        }
        
        return null;
    }
    
    public void ExecuteCommand(InputCommand command)
    {
        if (command != null && command.ClearsBuffer)
        {
            _inputHandler.InputBuffer.Clear();
        }
    }
    
    public void SetFacingDirection(int direction)
    {
        FacingDirection = direction;
    }
    
    public void AddCommand(InputCommand command)
    {
        Commands.Add(command);
        SortCommands();
    }
    
    public void RemoveCommand(InputCommand command)
    {
        Commands.Remove(command);
        SortCommands();
    }
}

// ==================== COMMON MOTIONS REFERENCE ====================
/*
Your 5-button system:
- Light, Medium, Heavy, Dash, Throw

Common Motions (Numpad Notation):
236   = Quarter Circle Forward (Hadouken)
214   = Quarter Circle Back
623   = Dragon Punch (DP/Shoryuken)
41236 = Half Circle Forward
63214 = Half Circle Back
632147 or 41236987 = 360
[4]6  = Charge Back, Forward (Sonic Boom)
[2]8  = Charge Down, Up (Flash Kick)

Double Motions for Supers:
236236 = Double QCF
214214 = Double QCB
623623 = Double DP

Movement:
4 or 6 = Walk
44 or 66 = Dash
44 (hold) or 66 (hold) = Run (if applicable)
7, 8, or 9 = Jump

Command Normals:
6L, 6M, 6H = Forward + button
3L, 3M, 3H = Down-forward + button
2L, 2M, 2H = Crouching attacks

Your Cancel Hierarchy:
Normal → Special → Super

Chains:
L → L (allowed)
M → H (allowed)
*/