using System.Collections.Generic;
using UnityEngine;
using System;

public class BoxManager : MonoBehaviour
{
    private List<CollisionBox> _activeBoxes = new List<CollisionBox>();
    private CharacterStateMachine _stateMachine;
    
    // Event for when a new attack starts (to clear hit tracking)
    public event Action OnNewAttackStarted;
    
    // For rendering boxes in game view
    [SerializeField] private bool _showBoxes = true;
    [SerializeField] private Material _boxMaterial;
    
    private void Awake()
    {
        _stateMachine = GetComponent<CharacterStateMachine>();
        
        // Create a simple unlit material for rendering boxes
        if (_boxMaterial == null)
        {
            _boxMaterial = new Material(Shader.Find("Sprites/Default"));
        }
    }
    
    public CollisionBox CreateHitbox(Vector2 offset, Vector2 size, int damage, Vector2 knockback, int hitStun)
    {
        GameObject boxObj = new GameObject("Hitbox");
        boxObj.transform.SetParent(transform);
        boxObj.transform.localPosition = Vector3.zero;
        boxObj.layer = LayerMask.NameToLayer("Default"); // Unity 2D physics uses layers differently
        
        CollisionBox box = boxObj.AddComponent<CollisionBox>();
        box.Type = BoxType.Hitbox;
        box.BoxRect = new Rect(offset, size);
        box.Damage = damage;
        box.Knockback = knockback;
        box.HitStun = hitStun;
        
        _activeBoxes.Add(box);
        return box;
    }
    
    public CollisionBox CreateHurtbox(Vector2 offset, Vector2 size)
    {
        GameObject boxObj = new GameObject("Hurtbox");
        boxObj.transform.SetParent(transform);
        boxObj.transform.localPosition = Vector3.zero;
        boxObj.layer = LayerMask.NameToLayer("Default");
        
        CollisionBox box = boxObj.AddComponent<CollisionBox>();
        box.Type = BoxType.Hurtbox;
        box.BoxRect = new Rect(offset, size);
        
        _activeBoxes.Add(box);
        return box;
    }
    
    public void ClearAllBoxes()
    {
        foreach (var box in _activeBoxes)
        {
            if (box != null)
                Destroy(box.gameObject);
        }
        _activeBoxes.Clear();
        
        // Notify that a new attack is starting
        OnNewAttackStarted?.Invoke();
    }
    
    public void ClearHitboxes()
    {
        for (int i = _activeBoxes.Count - 1; i >= 0; i--)
        {
            if (_activeBoxes[i].Type == BoxType.Hitbox)
            {
                Destroy(_activeBoxes[i].gameObject);
                _activeBoxes.RemoveAt(i);
            }
        }
    }
    
    public void ClearHurtboxes()
    {
        for (int i = _activeBoxes.Count - 1; i >= 0; i--)
        {
            if (_activeBoxes[i].Type == BoxType.Hurtbox)
            {
                Destroy(_activeBoxes[i].gameObject);
                _activeBoxes.RemoveAt(i);
            }
        }
    }
    
    // Render boxes in game view (training mode style)
    private void OnRenderObject()
    {
        if (!_showBoxes) return;
        
        foreach (var box in _activeBoxes)
        {
            if (box == null) continue;
            
            // Set color based on box type
            Color boxColor = box.Type == BoxType.Hitbox 
                ? new Color(1, 0, 0, 0.3f)  // Red with transparency
                : new Color(0, 0, 1, 0.3f); // Blue with transparency
            
            _boxMaterial.color = boxColor;
            _boxMaterial.SetPass(0);
            
            // Draw filled quad
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.QUADS);
            
            Vector3 center = new Vector3(box.BoxRect.center.x, box.BoxRect.center.y, 0);
            Vector3 size = new Vector3(box.BoxRect.size.x, box.BoxRect.size.y, 0);
            
            Vector3 topLeft = center + new Vector3(-size.x * 0.5f, size.y * 0.5f, 0);
            Vector3 topRight = center + new Vector3(size.x * 0.5f, size.y * 0.5f, 0);
            Vector3 bottomRight = center + new Vector3(size.x * 0.5f, -size.y * 0.5f, 0);
            Vector3 bottomLeft = center + new Vector3(-size.x * 0.5f, -size.y * 0.5f, 0);
            
            GL.Vertex(topLeft);
            GL.Vertex(topRight);
            GL.Vertex(bottomRight);
            GL.Vertex(bottomLeft);
            
            GL.End();
            GL.PopMatrix();
            
            // Draw outline
            Color outlineColor = box.Type == BoxType.Hitbox ? Color.red : Color.blue;
            _boxMaterial.color = outlineColor;
            _boxMaterial.SetPass(0);
            
            GL.PushMatrix();
            GL.MultMatrix(transform.localToWorldMatrix);
            GL.Begin(GL.LINES);
            
            // Top
            GL.Vertex(topLeft);
            GL.Vertex(topRight);
            // Right
            GL.Vertex(topRight);
            GL.Vertex(bottomRight);
            // Bottom
            GL.Vertex(bottomRight);
            GL.Vertex(bottomLeft);
            // Left
            GL.Vertex(bottomLeft);
            GL.Vertex(topLeft);
            
            GL.End();
            GL.PopMatrix();
        }
    }
}