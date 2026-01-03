using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInputBuffer
{
    private List<InputEvent> _inputHistory;
    private Dictionary<BufferInput, InputEvent> _currentlyHeldInputs;
    private int _bufferWindowFrames; // How many frames to keep inputs in history (e.g., 30 frames = 0.5 seconds at 60fps)
    
    public PlayerInputBuffer(int bufferWindowFrames = 30)
    {
        _inputHistory = new List<InputEvent>();
        _currentlyHeldInputs = new Dictionary<BufferInput, InputEvent>();
        _bufferWindowFrames = bufferWindowFrames;
    }
    
    public void OnInputPressed(BufferInput input)
    {
        if (_currentlyHeldInputs.ContainsKey(input))
            return;
            
        var inputEvent = new InputEvent(input, FrameCounter.CurrentFrame);
        _inputHistory.Add(inputEvent);
        _currentlyHeldInputs[input] = inputEvent;
    }
    
    public void OnInputReleased(BufferInput input)
    {
        if (!_currentlyHeldInputs.ContainsKey(input))
            return;
            
        var inputEvent = _currentlyHeldInputs[input];
        inputEvent.FrameReleased = FrameCounter.CurrentFrame;
        inputEvent.IsHeld = false;
        
        for (int i = _inputHistory.Count - 1; i >= 0; i--)
        {
            if (_inputHistory[i].FramePressed == inputEvent.FramePressed && 
                _inputHistory[i].Input == input)
            {
                _inputHistory[i] = inputEvent;
                break;
            }
        }
        
        _currentlyHeldInputs.Remove(input);
    }
    
    public void Update()
    {
        int currentFrame = FrameCounter.CurrentFrame;
        _inputHistory.RemoveAll(e => 
            !e.IsHeld && (currentFrame - e.FrameReleased) > _bufferWindowFrames
        );
    }
    
    public bool IsInputHeld(BufferInput input)
    {
        return _currentlyHeldInputs.ContainsKey(input);
    }
    
    public int GetHeldFrames(BufferInput input)
    {
        if (_currentlyHeldInputs.TryGetValue(input, out var inputEvent))
        {
            return inputEvent.HeldFrames;
        }
        return 0;
    }
    
    public bool WasInputPressed(BufferInput input, int withinFrames = 6)
    {
        int currentFrame = FrameCounter.CurrentFrame;
        for (int i = _inputHistory.Count - 1; i >= 0; i--)
        {
            if (_inputHistory[i].Input == input && 
                (currentFrame - _inputHistory[i].FramePressed) <= withinFrames)
            {
                return true;
            }
        }
        return false;
    }
    
    public List<InputEvent> GetInputHistory()
    {
        return new List<InputEvent>(_inputHistory);
    }
    
    public bool CheckMotionInput(List<BufferInput> sequence, int maxFrameWindow = 18)
    {
        if (sequence.Count == 0) return false;
        
        int currentFrame = FrameCounter.CurrentFrame;
        int sequenceIndex = 0;
        
        for (int i = 0; i < _inputHistory.Count; i++)
        {
            if (_inputHistory[i].Input == sequence[sequenceIndex])
            {
                if (sequenceIndex == 0)
                {
                    if (currentFrame - _inputHistory[i].FramePressed > maxFrameWindow)
                        continue;
                }
                
                sequenceIndex++;
                
                if (sequenceIndex == sequence.Count)
                    return true;
            }
        }
        
        return false;
    }
    
    public void Clear()
    {
        _inputHistory.Clear();
        _currentlyHeldInputs.Clear();
    }
}
