public class CharacterDashState : State
{
    // I image this class takes in a scriptable object that contains the dash animation and the dash speed, and have a seperate state for forward and backward dash
    public CharacterDashState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
    }

    public override void Update()
    {
        base.Update();
    }
    public override void Exit()
    {
        base.Exit();
        
    }
}
