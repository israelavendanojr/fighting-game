using System.Collections.Generic;
using UnityEngine;

public class MoveExecutor : MonoBehaviour
{
    [SerializeField] private CharacterStateMachine _stateMachine;
    [SerializeField] private PlayerInputHandler _inputHandler;
    [SerializeField] private List<CharacterMove> _availableMoves;
    
    private void Update()
    {
        // Only check for moves when in stand state (or other cancelable states)
        if (_stateMachine.CurrentState == _stateMachine.StandState)
        {
            TryExecuteMove();
        }
    }
    
    private void TryExecuteMove()
    {
        if (_inputHandler == null || _inputHandler.InputBuffer == null)
            return;
        
        // Sort moves by priority (higher priority first)
        _availableMoves.Sort((a, b) => b.commandInput.priority.CompareTo(a.commandInput.priority));
            

        
        foreach (var move in _availableMoves)
        {
            if (CheckMoveInput(move))
            {
                ExecuteMove(move);
                return;
            }
        }
    }
    
    private bool CheckMoveInput(CharacterMove move)
    {
        if (move.commandInput == null || move.commandInput.inputSequences.Length == 0)
            return false;

        
        // Check each possible input sequence for this move
        foreach (var sequence in move.commandInput.inputSequences)
        {
            if (CheckSequence(sequence, move.commandInput.maxFrameWindow))
            {
                return true;
            }
        }
        
        return false;
    }
    
    private bool CheckSequence(MotionSequence sequence, int maxFrameWindow)
    {
        var history = _inputHandler.InputBuffer.GetInputHistory();
        if (history.Count == 0) return false;
        
        int currentFrame = FrameCounter.CurrentFrame;
        int sequenceIndex = 0;
        int firstInputFrame = -1;
        
        // Check history in chronological order
        for (int i = 0; i < history.Count; i++)
        {
            var inputEvent = history[i];
            
            // If we haven't started matching yet, check if this is too old
            if (sequenceIndex == 0)
            {
                if (currentFrame - inputEvent.FramePressed > maxFrameWindow)
                    continue;
            }
            
            
            // Check if this input matches the current step in sequence
            if (inputEvent.Input == sequence.steps[sequenceIndex].input)
            {
                // Check if held long enough
                if (inputEvent.HeldFrames >= sequence.steps[sequenceIndex].minimumHoldFrames)
                {
                    if (sequenceIndex == 0)
                        firstInputFrame = inputEvent.FramePressed;
                    
                    sequenceIndex++;
                    
                    // Complete match found
                    if (sequenceIndex >= sequence.steps.Length)
                    {
                        // Verify entire sequence is within frame window
                        if (currentFrame - firstInputFrame <= maxFrameWindow)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        
        return false;
    }
    
    private void ExecuteMove(CharacterMove move)
    {
        // Debug.Log($"Executing move: {move.name}");
        
        // Get the appropriate move state and set its data
        CharacterMoveState moveState = GetMoveStateForMove(move);
        moveState.SetMoveData(move);
        
        // Clear buffer if specified
        if (move.commandInput.clearsBufferOnSuccess)
        {
            _inputHandler.InputBuffer.Clear();
        }
        
        // Transition to the move state
        _stateMachine.SetState(moveState);
    }
    
    private CharacterMoveState GetMoveStateForMove(CharacterMove move)
    {
        // For now, just use a generic move state
        // Later you could have specific states for different move types
        return _stateMachine.MoveState;
    }
}