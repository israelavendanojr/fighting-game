using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    // States
    public CharacterStandState StandState { get; private set; }
    public CharacterMoveState MoveState { get; private set; }
    
    
    // Expose current state for move checking
    public State CurrentState => _currentState;

    private void Awake()
    {
        // Initialize all states
        StandState = new CharacterStandState(this);
        MoveState = new CharacterMoveState(this);
        
    }

    public override State InitialState() => StandState;
}