using System.Diagnostics;
using UnityEngine;

public class CharacterStandState : CharacterBaseState
{
    private CharacterStateMachine _character;
    private BoxManager _boxManager;
    
    public CharacterStandState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
        _boxManager = _character.GetComponent<BoxManager>();
        
        // Add BoxManager if it doesn't exist
        if (_boxManager == null)
        {
            _boxManager = _character.gameObject.AddComponent<BoxManager>();
        }
    }

    public override void Enter()
    {
        base.Enter();
        
        // Clear any attack boxes
        _boxManager.ClearAllBoxes();
        
        // Create persistent hurtbox for standing state
        // Adjust these values based on your character's size
        // Since your character has scale (1, 2, 1), the hurtbox should cover the body
        _boxManager.CreateHurtbox(
            new Vector2(0, 0),      // Centered on character
            new Vector2(0.8f, 1.8f) // Slightly smaller than character for tight hitboxes
        );
        
        // UnityEngine.Debug.Log("Stand - Hurtbox created");
    }

    public override void Update()
    {
        base.Update();
        // Input checking is handled by MoveExecutor
    }

    public override void Exit()
    {
        base.Exit();
        // Don't clear hurtboxes here - let the next state handle it
    }
}