using UnityEngine;
public class GameFrameManager : MonoBehaviour
{
    [SerializeField] private int _targetFrameRate = 60;
    
    private void Awake()
    {
        Application.targetFrameRate = _targetFrameRate;
        FrameCounter.Reset();
    }
    
    private void FixedUpdate()
    {
        // Increment frame counter in FixedUpdate for deterministic timing
        FrameCounter.IncrementFrame();
    }
}