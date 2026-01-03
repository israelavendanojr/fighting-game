using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    // States
    public CharacterIdleState IdleState { get; private set; }
    public CharacterWalkState BackWalkState { get; private set; }
    public CharacterWalkState ForwardWalkState { get; private set; }
    public CharacterDashState BackDashState { get; private set; }
    public CharacterDashState ForwardDashState { get; private set; }

    [SerializeField] private CharacterWalkData _backWalkData;
    [SerializeField] private CharacterWalkData _forwardWalkData;
    [SerializeField] private CharacterDashData _backDashData;
    [SerializeField] private CharacterDashData _forwardDashData;

    private void Awake()
    {
        // Initialize all states
        IdleState = new CharacterIdleState(this);
        BackWalkState = new CharacterWalkState(this, _backWalkData);
        ForwardWalkState = new CharacterWalkState(this, _forwardWalkData);
        BackDashState = new CharacterDashState(this);
        ForwardDashState = new CharacterDashState(this);
    }

    public override State InitialState() => IdleState;
}


