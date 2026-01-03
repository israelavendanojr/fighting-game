public abstract class CharacterBaseState : State
{
    protected CharacterBaseState(CharacterStateMachine characterStateMachine) : base(characterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // TODO: Common base state enter logic
    }

    public override void Update()
    {
        base.Update();
        // TODO: Common base state update logic
    }

    public override void Exit()
    {
        base.Exit();
        // TODO: Common base state exit logic
    }
}