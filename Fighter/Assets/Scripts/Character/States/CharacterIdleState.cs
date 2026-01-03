using System.Diagnostics;
using UnityEngine;

public class CharacterIdleState : State
{
    private CharacterStateMachine _character;
    public CharacterIdleState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        // TODO: Set idle animation
        UnityEngine.Debug.Log("Idle");
        _character.SetState(_character.ForwardWalkState);
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