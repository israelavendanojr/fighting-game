using UnityEngine;

public enum BoxType
{
    Hitbox,
    Hurtbox
}

public class CollisionBox : MonoBehaviour
{
    public BoxType Type;
    public Rect BoxRect;
    public int Damage;
    public Vector2 Knockback;
    public int HitStun;
    
    private BoxCollider _collider;
    private CharacterStateMachine _owner;
    
    private void Awake()
    {
        _collider = gameObject.AddComponent<BoxCollider>();
        _collider.isTrigger = true;
        _owner = GetComponentInParent<CharacterStateMachine>();
        UpdateCollider();
    }
    
    public void UpdateCollider()
    {
        if (_collider == null) return;
        
        _collider.center = new Vector3(BoxRect.center.x, BoxRect.center.y, 0);
        _collider.size = new Vector3(BoxRect.size.x, BoxRect.size.y, 0.1f);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Only process hitbox collisions
        if (Type != BoxType.Hitbox) return;
        
        CollisionBox otherBox = other.GetComponent<CollisionBox>();
        if (otherBox == null || otherBox.Type != BoxType.Hurtbox) return;
        
        // Don't hit yourself
        CharacterStateMachine otherOwner = otherBox.GetComponentInParent<CharacterStateMachine>();
        if (otherOwner == null || otherOwner == _owner) return;
        
        // Don't hit opponents already in hitstun (optional - remove for juggle combos)
        if (otherOwner.CurrentState is CharacterHitstunState) return;
        
        // Apply the hit
        ApplyHit(otherOwner);
    }
    
    private void ApplyHit(CharacterStateMachine opponent)
    {
        // Calculate knockback direction based on attacker and defender positions
        Vector3 attackerPos = _owner.transform.position;
        Vector3 defenderPos = opponent.transform.position;
        float direction = Mathf.Sign(defenderPos.x - attackerPos.x);
        
        // Apply direction to knockback
        Vector2 adjustedKnockback = new Vector2(
            Knockback.x * direction,
            Knockback.y
        );
        
        // Transition opponent to hitstun
        CharacterHitstunState hitstunState = opponent.HitstunState;
        hitstunState.SetHitstunData(HitStun, adjustedKnockback, Damage);
        opponent.SetState(hitstunState);
        
        Debug.Log($"Hit! Damage: {Damage}, Knockback: {adjustedKnockback}, Hitstun: {HitStun}f");
    }
    
    private void OnDrawGizmos()
    {
        // Draw the box in the editor
        Gizmos.color = Type == BoxType.Hitbox ? new Color(1, 0, 0, 0.5f) : new Color(0, 0, 1, 0.5f);
        
        Vector3 center = transform.position + new Vector3(BoxRect.center.x, BoxRect.center.y, 0);
        Gizmos.DrawCube(center, new Vector3(BoxRect.size.x, BoxRect.size.y, 0.1f));
        
        // Draw outline
        Gizmos.color = Type == BoxType.Hitbox ? Color.red : Color.blue;
        Gizmos.DrawWireCube(center, new Vector3(BoxRect.size.x, BoxRect.size.y, 0.1f));
    }
}