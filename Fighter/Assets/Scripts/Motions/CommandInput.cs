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

