using UnityEngine;

public class CharacterWalkState : State
{
    private CharacterStateMachine _character;
    private CharacterWalkData _walkData;
    
    public CharacterWalkState(CharacterStateMachine stateMachine, CharacterWalkData walkData) : base(stateMachine)
    {
        _character = stateMachine;
        _walkData = walkData;
    }

    public override void Enter()
    {
        base.Enter();
        // TODO: Set walk animation based on direction
        // if (_walkData.Direction > 0) play forward walk animation
        // else play backward walk animation
    }

    public override void Update()
    {
        base.Update();
        
        // Deterministic movement using Transform and Time.deltaTime
        // This will work consistently across different framerates
        float movement = _walkData.WalkSpeed * _walkData.Direction * Time.deltaTime;
        _character.transform.position += new Vector3(movement, 0f, 0f);
        
        // TODO: Check for input to transition to other states
        // - If no input, return to idle
        // - If dash input, transition to dash state
    }

    public override void Exit()
    {
        base.Exit();
    }
}