using System.Diagnostics;
using UnityEngine;

public class CharacterMoveState : CharacterBaseState
{
    private CharacterStateMachine _character;
    public CharacterMoveState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        // TODO: Set idle animation
        UnityEngine.Debug.Log("Attack");
    }

    public override void Update()
    {
        base.Update();
        // TODO: Check for input to transition to other states
    }

    public override void Exit()
    {
        base.Exit();
    }
}