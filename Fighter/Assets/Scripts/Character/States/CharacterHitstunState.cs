using UnityEngine;

public class CharacterHitstunState : CharacterBaseState
{
    private CharacterStateMachine _character;
    private int _currentFrame;
    private int _hitstunDuration;
    private Vector2 _knockbackVelocity;
    private Vector2 _currentVelocity;
    private HealthComponent _healthComponent;
    private HitboxCollisionDetector _collisionDetector;
    
    // Knockback decay for realistic physics
    private const float KNOCKBACK_DECAY = 0.92f;
    private const float GRAVITY = -0.5f;
    private const float GROUND_Y = 1f; // Adjust based on your character's ground position
    
    public CharacterHitstunState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
        _character = characterStateMachine;
        _healthComponent = _character.GetComponent<HealthComponent>();
        _collisionDetector = _character.GetComponent<HitboxCollisionDetector>();
        
        // Add collision detector if it doesn't exist
        if (_collisionDetector == null)
        {
            _collisionDetector = _character.gameObject.AddComponent<HitboxCollisionDetector>();
        }
    }
    
    public void SetHitstunData(int duration, Vector2 knockback, int damage)
    {
        _hitstunDuration = duration;
        _knockbackVelocity = knockback;
        _currentVelocity = knockback;
        
        // Apply damage when hitstun is set
        if (_healthComponent != null && damage > 0)
        {
            _healthComponent.TakeDamage(damage);
        }
    }

    public override void Enter()
    {
        base.Enter();
        _currentFrame = 0;
        
        // Clear any active boxes when entering hitstun
        var boxManager = _character.GetComponent<BoxManager>();
        if (boxManager != null)
        {
            boxManager.ClearAllBoxes();
        }
        
        UnityEngine.Debug.Log($"Entered hitstun: {_hitstunDuration} frames, knockback: {_knockbackVelocity}");
    }

    public override void Update()
    {
        base.Update();
        _currentFrame++;
        
        // Check if hitstun is over
        if (_currentFrame >= _hitstunDuration)
        {
            // Check if character is grounded
            bool isGrounded = _character.transform.position.y <= GROUND_Y;
            
            if (isGrounded)
            {
                // Snap to ground and return to stand
                Vector3 pos = _character.transform.position;
                pos.y = GROUND_Y;
                _character.transform.position = pos;
                
                _character.SetState(_character.StandState);
            }
        }
    }
    
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        // Apply knockback physics deterministically
        ApplyKnockback();
    }
    
    private void ApplyKnockback()
    {
        // Apply gravity to vertical velocity
        _currentVelocity.y += GRAVITY * Time.fixedDeltaTime;
        
        // Apply horizontal decay (friction)
        _currentVelocity.x *= KNOCKBACK_DECAY;
        
        // Calculate movement delta (deterministic using fixed timestep)
        Vector2 movement = _currentVelocity * Time.fixedDeltaTime;
        
        // Apply movement
        Vector3 newPosition = _character.transform.position;
        newPosition.x += movement.x;
        newPosition.y += movement.y;
        
        // Ground check - prevent going below ground
        if (newPosition.y < GROUND_Y)
        {
            newPosition.y = GROUND_Y;
            _currentVelocity.y = 0; // Stop vertical velocity when hitting ground
            
            // Reduce horizontal velocity significantly on ground bounce
            _currentVelocity.x *= 0.5f;
        }
        
        _character.transform.position = newPosition;
    }

    public override void Exit()
    {
        base.Exit();
        _currentVelocity = Vector2.zero;
        UnityEngine.Debug.Log("Exited hitstun");
    }
}