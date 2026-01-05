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