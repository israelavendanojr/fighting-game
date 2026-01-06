using System.Collections.Generic;
using UnityEngine;

public class MoveExecutor : MonoBehaviour
{
    [SerializeField] private CharacterStateMachine _stateMachine;
    [SerializeField] private PlayerInputHandler _inputHandler;
    
    [Header("Attacks")]
    [SerializeField] private List<CharacterMove> _availableMoves;
    
    [Header("Movement")]
    [SerializeField] private List<CharacterMovement> _availableMovements;
    
    private void Update()
    {
        // 1. Check what state we're in and what transitions are allowed
        bool canCheckAttacks = CanCheckAttacks();
        bool canCheckMovement = CanCheckMovement();
        
        // 2. Try attacks first (highest priority)
        if (canCheckAttacks)
        {
            if (TryExecuteMove())
                return;
        }
        
        // 3. Then try movement
        if (canCheckMovement)
        {
            if (TryExecuteMovement())
                return;
        }
        
        // 4. If no movement input and we're in the walk state, return to stand
        if (_stateMachine.CurrentState == _stateMachine.WalkState)
        {
            if (!HasMovementInput())
            {
                _stateMachine.SetState(_stateMachine.StandState);
            }
        }
    }
    
    private bool CanCheckAttacks()
    {
        // Always allowed from stand
        if (_stateMachine.CurrentState == _stateMachine.StandState)
            return true;
        
        // Check if current movement state allows attack transitions
        if (_stateMachine.CurrentState is CharacterMovementState movementState)
            return movementState.CanTransitionToAttacks();
        
        return false;
    }
    
    private bool CanCheckMovement()
    {
        // Always allowed from stand
        if (_stateMachine.CurrentState == _stateMachine.StandState)
            return true;
        
        // Check if current movement state allows transitions to other movement
        if (_stateMachine.CurrentState is CharacterMovementState movementState)
            return movementState.CanTransitionToOtherMovement();
        
        return false;
    }
    
    private bool HasMovementInput()
    {
        if (_inputHandler == null) return false;
        Vector2 moveInput = _inputHandler.GetMoveInput();
        // Return true if there is significant horizontal input
        return Mathf.Abs(moveInput.x) > 0.1f;
    }
    
    private bool TryExecuteMove()
    {
        if (_inputHandler == null || _inputHandler.InputBuffer == null)
            return false;
        
        // Sort moves by priority
        _availableMoves.Sort((a, b) => b.commandInput.priority.CompareTo(a.commandInput.priority));
        
        foreach (var move in _availableMoves)
        {
            if (CheckMoveInput(move))
            {
                ExecuteMove(move);
                return true;
            }
        }
        
        return false;
    }
    
    private bool TryExecuteMovement()
    {
        if (_inputHandler == null || _inputHandler.InputBuffer == null)
            return false;
        
        // Sort movements by priority
        _availableMovements.Sort((a, b) => b.commandInput.priority.CompareTo(a.commandInput.priority));
        
        foreach (var movement in _availableMovements)
        {
            if (CheckMovementInput(movement))
            {
                ExecuteMovement(movement);
                return true;
            }
        }
        
        return false;
    }
    
    private bool CheckMoveInput(CharacterMove move)
    {
        if (move.commandInput == null || move.commandInput.inputSequences.Length == 0)
            return false;
        
        foreach (var sequence in move.commandInput.inputSequences)
        {
            if (CheckSequence(sequence, move.commandInput.maxFrameWindow))
                return true;
        }
        return false;
    }
    
    private bool CheckMovementInput(CharacterMovement movement)
    {
        if (movement.commandInput == null || movement.commandInput.inputSequences.Length == 0)
            return false;
        
        foreach (var sequence in movement.commandInput.inputSequences)
        {
            if (CheckSequence(sequence, movement.commandInput.maxFrameWindow))
                return true;
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
        
        // Iterates through input history to match the motion sequence
        for (int i = 0; i < history.Count; i++)
        {
            var inputEvent = history[i];
            
            if (sequenceIndex == 0)
            {
                if (currentFrame - inputEvent.FramePressed > maxFrameWindow)
                    continue;
            }
            
            if (inputEvent.Input == sequence.steps[sequenceIndex].input)
            {
                if (inputEvent.HeldFrames >= sequence.steps[sequenceIndex].minimumHoldFrames)
                {
                    if (sequenceIndex == 0)
                        firstInputFrame = inputEvent.FramePressed;
                    
                    sequenceIndex++;
                    
                    if (sequenceIndex >= sequence.steps.Length)
                    {
                        if (currentFrame - firstInputFrame <= maxFrameWindow)
                            return true;
                    }
                }
            }
        }
        return false;
    }
    
    private void ExecuteMove(CharacterMove move)
    {
        CharacterMoveState moveState = _stateMachine.MoveState;
        
        // Inject data before transition
        moveState.SetMoveData(move);
        
        if (move.commandInput.clearsBufferOnSuccess)
            _inputHandler.InputBuffer.Clear();
        
        if (_stateMachine.CurrentState != moveState)
            _stateMachine.SetState(moveState);
    }
    
    private void ExecuteMovement(CharacterMovement movement)
    {
        // Routes all walking data (forward/back) to the same WalkState
        CharacterMovementState walkState = _stateMachine.WalkState;
        
        // Update the state with the specific movement SO (Speed, Friction, etc.)
        walkState.SetMovementData(movement);
        
        if (movement.commandInput.clearsBufferOnSuccess)
            _inputHandler.InputBuffer.Clear();
        
        if (_stateMachine.CurrentState != walkState)
            _stateMachine.SetState(walkState);
    }
}