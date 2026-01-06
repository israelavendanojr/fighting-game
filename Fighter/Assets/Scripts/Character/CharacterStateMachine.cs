using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    // States
    public CharacterStandState StandState { get; private set; }
    public CharacterMoveState MoveState { get; private set; }
    public CharacterHitstunState HitstunState { get; private set; }
    
    // Movement states
    public CharacterMovementState ForwardWalkState { get; private set; }
    public CharacterMovementState BackWalkState { get; private set; }
    public CharacterMovementState ForwardDashState { get; private set; }
    public CharacterMovementState BackDashState { get; private set; }
    
    // Expose current state for move checking
    public State CurrentState => _currentState;

    private void Awake()
    {
        // Initialize all states
        StandState = new CharacterStandState(this);
        MoveState = new CharacterMoveState(this);
        HitstunState = new CharacterHitstunState(this);
        
        // Initialize movement states
        ForwardWalkState = new CharacterMovementState(this);
        BackWalkState = new CharacterMovementState(this);
        ForwardDashState = new CharacterMovementState(this);
        BackDashState = new CharacterMovementState(this);
    }

    public override State InitialState() => StandState;
}