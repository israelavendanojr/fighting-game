using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCommandInput", menuName = "Character/Command Input")]
public class CommandInput : ScriptableObject
{
    public MotionSequence[] inputSequences; 
    
    public int maxFrameWindow = 18;
    public bool clearsBufferOnSuccess = true;
    public int priority = 0;
}

[System.Serializable]
public class MotionSequence
{
    public MotionStep[] steps;
}

[System.Serializable]
public class MotionStep
{
    public BufferInput input = BufferInput.Neutral;
    public int minimumHoldFrames = 1;
}