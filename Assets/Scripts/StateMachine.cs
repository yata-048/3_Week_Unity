public class StateMachine
{
    public IState CurrentState { get; private set; }

    public void Tick()
    {
        CurrentState?.Tick();
    }

    public void FixedTick()
    {
        CurrentState?.FixedTick();
    }
    
    public void ChangeState(IState targetState)
    {
        if(CurrentState == targetState) return;
        CurrentState?.Exit();
        CurrentState = targetState;
        CurrentState?.Enter();
    }
    
    public void ForceSetState(IState state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState?.Enter();
    }
}