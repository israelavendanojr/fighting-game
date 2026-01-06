using UnityEngine;

public class CharacterMovementState : CharacterBaseState
{
    private CharacterStateMachine _character;
    private CharacterMovement _movementData;
    
    public CharacterMovementState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
    }
    
    public void SetMovementData(CharacterMovement movementData)
    {
        _movementData = movementData;
    }

    public override void Enter()
    {
        base.Enter();
        
        if (_movementData == null)
        {
            UnityEngine.Debug.LogError("CharacterMovementState entered without movement data!");
            _character.SetState(_character.StandState);
            return;
        }
        
        // UnityEngine.Debug.Log($"MOVEMENT STATE: Executing movement: {_movementData.name}");
        
        // TODO: Play animation if available
        // if (_movementData.animationClip != null)
        //     Play animation
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (_movementData == null) return;
        
        // Apply movement in FixedUpdate for determinism
        ApplyMovement();
    }
    
    private void ApplyMovement()
    {
        // Get the input handler to check current directional input
        var inputHandler = _character.GetComponent<PlayerInputHandler>();
        if (inputHandler == null) return;
        
        Vector2 moveInput = inputHandler.GetMoveInput();
        
        // Calculate movement delta
        float moveX = moveInput.x * _movementData.speed * Time.fixedDeltaTime;
        
        // Apply friction if no input
        if (Mathf.Abs(moveInput.x) < 0.1f)
        {
            moveX *= _movementData.friction;
        }
        
        // Apply movement using transform (deterministic)
        _character.transform.position += new Vector3(moveX, 0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        _movementData = null;
    }
    
    public bool CanTransitionToAttacks()
    {
        return _movementData != null && _movementData.canTransitionToAttacks;
    }
    
    public bool CanTransitionToOtherMovement()
    {
        return _movementData != null && _movementData.canTransitionToOtherMovement;
    }
}