namespace MiniTowerDefence.StateMachine
{
    public interface IState
    {
        IState Transition();
        void Enter();
        void Exit();
    }
}