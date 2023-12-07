namespace MiniTowerDefence.StateMachine
{
    public class StateMachine
    {
        public StateMachine(IState startingState) => ChangeState(startingState);

        public IState CurrentState { get; private set; }

        public void ChangeState(IState state)
        {
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState?.Enter();
        }

        public void Tick()
        {
            var nextState = CurrentState.Transition();

            if (nextState != null)
            {
                ChangeState(nextState);
            }
        }
    }
}