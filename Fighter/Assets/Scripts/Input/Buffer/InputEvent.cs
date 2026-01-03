public struct InputEvent
{
    public BufferInput Input;
    public int FramePressed;    
    public int FrameReleased;   
    public bool IsHeld;          
    
    public int HeldFrames => IsHeld ? FrameCounter.CurrentFrame - FramePressed : FrameReleased - FramePressed;
    
    public InputEvent(BufferInput input, int framePressed)
    {
        Input = input;
        FramePressed = framePressed;
        FrameReleased = 0;
        IsHeld = true;
    }
}