using UnityEngine;

/// <summary>
/// Attach this to both player objects temporarily to debug collision detection.
/// Remove once everything works.
/// </summary>
public class CollisionDebugger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[{gameObject.name}] Main collider detected: {other.gameObject.name}");
    }
    
    private void Update()
    {
        // Draw a sphere at the player's position
        Debug.DrawRay(transform.position, Vector3.up * 2f, Color.yellow);
    }
    
    private void OnDrawGizmos()
    {
        // Draw the player's main collider
        var collider = GetComponent<BoxCollider>();
        if (collider != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(collider.center, collider.size);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(collider.center, collider.size);
        }
    }
}