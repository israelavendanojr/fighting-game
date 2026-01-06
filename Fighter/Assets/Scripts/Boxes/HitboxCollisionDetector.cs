using System.Collections.Generic;
using UnityEngine;

public class HitboxCollisionDetector : MonoBehaviour
{
    private BoxManager _boxManager;
    private HashSet<CollisionBox> _hitThisAttack = new HashSet<CollisionBox>();
    
    private void Awake()
    {
        _boxManager = GetComponent<BoxManager>();
        
        if (_boxManager != null)
        {
            _boxManager.OnNewAttackStarted += ClearHitList;
        }
    }
    
    private void OnDestroy()
    {
        if (_boxManager != null)
        {
            _boxManager.OnNewAttackStarted -= ClearHitList;
        }
    }
    
    private void ClearHitList()
    {
        _hitThisAttack.Clear();
    }
}