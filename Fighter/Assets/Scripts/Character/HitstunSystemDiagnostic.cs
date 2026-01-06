using UnityEngine;

/// <summary>
/// Diagnostic tool to verify hitstun system is properly set up.
/// Attach to any GameObject and check the console for setup issues.
/// </summary>
public class HitstunSystemDiagnostic : MonoBehaviour
{
    [Header("Run diagnostic on Start")]
    [SerializeField] private bool _runOnStart = true;
    
    [Header("References to Check")]
    [SerializeField] private GameObject[] _playerObjects;
    
    private void Start()
    {
        if (_runOnStart)
        {
            RunDiagnostic();
        }
    }
    
    [ContextMenu("Run Diagnostic")]
    public void RunDiagnostic()
    {
        Debug.Log("=== HITSTUN SYSTEM DIAGNOSTIC ===\n");
        
        // Find all players if not assigned
        if (_playerObjects == null || _playerObjects.Length == 0)
        {
            _playerObjects = GameObject.FindGameObjectsWithTag("Untagged"); // Adjust tag if needed
            Debug.Log($"Auto-found {_playerObjects.Length} potential player objects");
        }
        
        bool hasErrors = false;
        
        // Check Game Manager
        var gameManager = FindObjectOfType<GameFrameManager>();
        if (gameManager == null)
        {
            Debug.LogError("❌ GameFrameManager not found in scene! Frame counting won't work.");
            hasErrors = true;
        }
        else
        {
            Debug.Log("✓ GameFrameManager found");
        }
        
        // Check each player
        foreach (var playerObj in _playerObjects)
        {
            if (playerObj == null) continue;
            if (!playerObj.name.Contains("Player")) continue; // Skip non-player objects
            
            Debug.Log($"\n--- Checking: {playerObj.name} ---");
            
            CheckPlayerSetup(playerObj, ref hasErrors);
            CheckMoveData(playerObj, ref hasErrors);
        }
        
        // Physics settings
        Debug.Log("\n--- Physics Settings ---");
        Debug.Log($"Fixed Timestep: {Time.fixedDeltaTime} (should be ~0.0166 for 60fps)");
        
        if (hasErrors)
        {
            Debug.LogWarning("\n⚠️ DIAGNOSTIC COMPLETE - Issues found! Check logs above.");
        }
        else
        {
            Debug.Log("\n✅ DIAGNOSTIC COMPLETE - All systems operational!");
        }
    }
    
    private void CheckPlayerSetup(GameObject player, ref bool hasErrors)
    {
        // Check CharacterStateMachine
        var stateMachine = player.GetComponent<CharacterStateMachine>();
        if (stateMachine == null)
        {
            Debug.LogError($"❌ {player.name}: Missing CharacterStateMachine!");
            hasErrors = true;
        }
        else
        {
            Debug.Log($"✓ CharacterStateMachine");
            
            // Check if hitstun state exists
            if (stateMachine.HitstunState == null)
            {
                Debug.LogError($"❌ {player.name}: HitstunState is null! Script may not be updated.");
                hasErrors = true;
            }
            else
            {
                Debug.Log($"  ✓ HitstunState initialized");
            }
        }
        
        // Check BoxCollider
        var collider = player.GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.LogError($"❌ {player.name}: Missing BoxCollider!");
            hasErrors = true;
        }
        else
        {
            Debug.Log($"✓ BoxCollider found");
            
            if (!collider.isTrigger)
            {
                Debug.LogError($"❌ {player.name}: BoxCollider 'Is Trigger' is OFF! Turn it ON.");
                hasErrors = true;
            }
            else
            {
                Debug.Log($"  ✓ Is Trigger: ON");
            }
        }
        
        // Check HealthComponent
        var health = player.GetComponent<HealthComponent>();
        if (health == null)
        {
            Debug.LogError($"❌ {player.name}: Missing HealthComponent!");
            hasErrors = true;
        }
        else
        {
            Debug.Log($"✓ HealthComponent (HP: {health.GetCurrentHealth()}/{health.GetMaxHealth()})");
            
            if (health.GetMaxHealth() <= 0)
            {
                Debug.LogWarning($"⚠️ {player.name}: MaxHealth is 0! Set it to 100+");
            }
        }
        
        // Check PlayerInputHandler
        var inputHandler = player.GetComponent<PlayerInputHandler>();
        if (inputHandler == null)
        {
            Debug.LogError($"❌ {player.name}: Missing PlayerInputHandler!");
            hasErrors = true;
        }
        else
        {
            Debug.Log($"✓ PlayerInputHandler");
        }
        
        // Check MoveExecutor
        var executor = player.GetComponent<MoveExecutor>();
        if (executor == null)
        {
            Debug.LogError($"❌ {player.name}: Missing MoveExecutor!");
            hasErrors = true;
        }
        else
        {
            Debug.Log($"✓ MoveExecutor");
        }
        
        // Check BoxManager (auto-added)
        var boxManager = player.GetComponent<BoxManager>();
        if (boxManager != null)
        {
            Debug.Log($"✓ BoxManager (auto-added)");
        }
        
        // Check HitboxCollisionDetector (auto-added)
        var collisionDetector = player.GetComponent<HitboxCollisionDetector>();
        if (collisionDetector != null)
        {
            Debug.Log($"✓ HitboxCollisionDetector (auto-added)");
        }
    }
    
    private void CheckMoveData(GameObject player, ref bool hasErrors)
    {
        var executor = player.GetComponent<MoveExecutor>();
        if (executor == null) return;
        
        // Use reflection to access private field (for diagnostic only)
        var field = executor.GetType().GetField("_availableMoves", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field == null) return;
        
        var moves = field.GetValue(executor) as System.Collections.IList;
        if (moves == null || moves.Count == 0)
        {
            Debug.LogWarning($"⚠️ {player.name}: No moves assigned to MoveExecutor!");
            return;
        }
        
        Debug.Log($"\n--- Move Data Check ({player.name}) ---");
        
        int movesWithoutHitstun = 0;
        
        foreach (var move in moves)
        {
            if (move == null) continue;
            
            // Get attacks array
            var attacksField = move.GetType().GetField("attacks");
            if (attacksField == null) continue;
            
            var attacks = attacksField.GetValue(move) as System.Array;
            if (attacks == null || attacks.Length == 0) continue;
            
            // Check first attack's hitstun
            var firstAttack = attacks.GetValue(0);
            var hitstunField = firstAttack.GetType().GetField("hitStun");
            
            if (hitstunField != null)
            {
                int hitstun = (int)hitstunField.GetValue(firstAttack);
                string moveName = (move as ScriptableObject)?.name ?? "Unknown Move";
                
                if (hitstun <= 0)
                {
                    Debug.LogWarning($"⚠️ {moveName}: hitStun = {hitstun} (should be 10-30)");
                    movesWithoutHitstun++;
                }
                else
                {
                    Debug.Log($"✓ {moveName}: hitStun = {hitstun}");
                }
            }
        }
        
        if (movesWithoutHitstun > 0)
        {
            Debug.LogWarning($"\n⚠️ {movesWithoutHitstun} moves have hitStun = 0! " +
                           $"Update them in Unity Inspector (set to 15-30)");
            hasErrors = true;
        }
    }
}