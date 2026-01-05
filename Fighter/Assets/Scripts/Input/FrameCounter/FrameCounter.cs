public static class FrameCounter
{
    public static int CurrentFrame { get; private set; }
    
    public static void IncrementFrame()
    {
        CurrentFrame++;
    }
    
    public static void Reset()
    {
        CurrentFrame = 0;
    }
}