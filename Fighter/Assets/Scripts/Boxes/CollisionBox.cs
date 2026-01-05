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
    
    private BoxCollider2D _collider;
    
    private void Awake()
    {
        _collider = gameObject.AddComponent<BoxCollider2D>();
        _collider.isTrigger = true;
        UpdateCollider();
    }
    
    public void UpdateCollider()
    {
        if (_collider == null) return;
        
        _collider.offset = BoxRect.center;
        _collider.size = BoxRect.size;
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