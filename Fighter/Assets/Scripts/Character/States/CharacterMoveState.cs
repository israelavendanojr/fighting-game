using System.Diagnostics;
using UnityEngine;

public class CharacterMoveState : CharacterBaseState
{
    private CharacterStateMachine _character;
    private CharacterMove _moveData;
    private int _currentFrame;
    private int _currentAttackIndex;
    private BoxManager _boxManager;
    
    // Track which phase we're in for proper box management
    private enum AttackPhase { Startup, Active, Recovery }
    private AttackPhase _currentPhase;
    
    public CharacterMoveState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
        _boxManager = _character.GetComponent<BoxManager>();
        
        // Add BoxManager if it doesn't exist
        if (_boxManager == null)
        {
            _boxManager = _character.gameObject.AddComponent<BoxManager>();
        }
    }
    
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
        _currentPhase = AttackPhase.Startup;
        
        // Clear any existing boxes
        _boxManager.ClearAllBoxes();
        
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
            int totalFrames = GetTotalFrames();
            if (_currentFrame >= totalFrames)
            {
                _boxManager.ClearAllBoxes();
                _character.SetState(_character.StandState);
                return;
            }
        }
        
        // Process current attack phase
        ProcessCurrentAttack();
    }
    
    private void ProcessCurrentAttack()
    {
        if (_currentAttackIndex >= _moveData.attacks.Length) return;
        
        AttackData currentAttack = _moveData.attacks[_currentAttackIndex];
        int attackStartFrame = GetAttackStartFrame(_currentAttackIndex);
        int frameInAttack = _currentFrame - attackStartFrame;
        
        if (frameInAttack < currentAttack.startup)
        {
            // Startup phase - hurtboxes only, no hitboxes
            if (_currentPhase != AttackPhase.Startup)
            {
                _currentPhase = AttackPhase.Startup;
                _boxManager.ClearAllBoxes();
                SpawnHurtboxes(currentAttack);
            }
        }
        else if (frameInAttack < currentAttack.startup + currentAttack.active)
        {
            // Active phase - both hitboxes and hurtboxes
            if (_currentPhase != AttackPhase.Active)
            {
                _currentPhase = AttackPhase.Active;
                _boxManager.ClearAllBoxes();
                SpawnHitboxes(currentAttack);
                SpawnHurtboxes(currentAttack);
            }
        }
        else if (frameInAttack < currentAttack.startup + currentAttack.active + currentAttack.recovery)
        {
            // Recovery phase - hurtboxes only, no hitboxes
            if (_currentPhase != AttackPhase.Recovery)
            {
                _currentPhase = AttackPhase.Recovery;
                _boxManager.ClearAllBoxes();
                SpawnHurtboxes(currentAttack);
            }
        }
        else
        {
            // Move to next attack in sequence
            _currentAttackIndex++;
            _currentPhase = AttackPhase.Startup;
            _boxManager.ClearAllBoxes();
        }
    }
    
    private void SpawnHitboxes(AttackData attack)
    {
        if (attack.hitboxes == null) return;
        
        foreach (var hitboxData in attack.hitboxes)
        {
            _boxManager.CreateHitbox(
                hitboxData.offset,
                hitboxData.size,
                attack.damage,
                attack.knockback
            );
        }
    }
    
    private void SpawnHurtboxes(AttackData attack)
    {
        if (attack.hurtboxes == null) return;
        
        foreach (var hurtboxData in attack.hurtboxes)
        {
            _boxManager.CreateHurtbox(
                hurtboxData.offset,
                hurtboxData.size
            );
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
        _boxManager.ClearAllBoxes();
        _moveData = null;
    }
}