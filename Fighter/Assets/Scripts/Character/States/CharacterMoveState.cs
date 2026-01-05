using System.Diagnostics;
using UnityEngine;

public class CharacterMoveState : CharacterBaseState
{
    private CharacterStateMachine _character;
    private CharacterMove _moveData;
    private int _currentFrame;
    private int _currentAttackIndex;
    
    public CharacterMoveState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
    }
    
    // Set the move data when transitioning to this state
    public void SetMoveData(CharacterMove moveData)
    {
        _moveData = moveData;
    }

    public override void Enter()
    {
        base.Enter();
        
        if (_moveData == null)
        {
            UnityEngine.Debug.LogError("CharacterMoveState entered without move data!");
            _character.SetState(_character.StandState);
            return;
        }
        
        _currentFrame = 0;
        _currentAttackIndex = 0;
        
        // Play animation if available
        // TODO: Play _moveData.animationClip
        
        // Apply initial velocity
        // TODO: Apply _moveData.initialVelocity to character
        
        UnityEngine.Debug.Log($"Executing move: {_moveData.name}");
    }

    public override void Update()
    {
        base.Update();
        
        if (_moveData == null) return;
        
        _currentFrame++;
        
        // Check if we've completed all attacks
        if (_currentAttackIndex >= _moveData.attacks.Length)
        {
            // Check total frames
            int totalFrames = GetTotalFrames();
            if (_currentFrame >= totalFrames)
            {
                _character.SetState(_character.StandState);
                return;
            }
        }
        
        // Process current attack phase
        ProcessCurrentAttack();
        
        // TODO: Check for cancel windows
        // If in active or recovery frames and move is cancelable, check for new inputs
    }
    
    private void ProcessCurrentAttack()
    {
        if (_currentAttackIndex >= _moveData.attacks.Length) return;
        
        AttackData currentAttack = _moveData.attacks[_currentAttackIndex];
        int attackStartFrame = GetAttackStartFrame(_currentAttackIndex);
        int frameInAttack = _currentFrame - attackStartFrame;
        
        if (frameInAttack < currentAttack.startup)
        {
            // Startup phase
            // TODO: Show hurtboxes, no hitboxes yet
        }
        else if (frameInAttack < currentAttack.startup + currentAttack.active)
        {
            // Active phase
            // TODO: Show both hitboxes and hurtboxes, detect hits
        }
        else if (frameInAttack < currentAttack.startup + currentAttack.active + currentAttack.recovery)
        {
            // Recovery phase
            // TODO: Show hurtboxes, no hitboxes
        }
        else
        {
            // Move to next attack in sequence
            _currentAttackIndex++;
        }
    }
    
    private int GetAttackStartFrame(int attackIndex)
    {
        int frame = 0;
        for (int i = 0; i < attackIndex; i++)
        {
            frame += _moveData.attacks[i].startup + 
                     _moveData.attacks[i].active + 
                     _moveData.attacks[i].recovery;
        }
        return frame;
    }
    
    private int GetTotalFrames()
    {
        int total = 0;
        foreach (var attack in _moveData.attacks)
        {
            total += attack.startup + attack.active + attack.recovery;
        }
        return total;
    }

    public override void Exit()
    {
        base.Exit();
        _moveData = null;
    }
}