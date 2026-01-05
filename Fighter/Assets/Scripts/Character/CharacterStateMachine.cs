using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    // States
    public CharacterStandState StandState { get; private set; }
    public CharacterMoveState LightAttackState { get; private set; }
    public CharacterMoveState MediumAttackState { get; private set; }
    public CharacterMoveState HeavyAttackState { get; private set; }
    public CharacterMoveState SpecialOneState { get; private set; }
    public CharacterMoveState SpecialTwoState { get; private set; }
    
    // Expose current state for move checking
    public State CurrentState => _currentState;

    private void Awake()
    {
        // Initialize all states
        StandState = new CharacterStandState(this);
        LightAttackState = new CharacterMoveState(this);
        MediumAttackState = new CharacterMoveState(this);
        HeavyAttackState = new CharacterMoveState(this);
        SpecialOneState = new CharacterMoveState(this);
        SpecialTwoState = new CharacterMoveState(this);
    }

    public override State InitialState() => StandState;
}