namespace TSLib.AI.Behaviour.StateMachines
{
    public interface IStateMachine
    {
        public void Execute(float deltaTime);

        public void TransitionTo(TS_State newState, bool doEnter, bool doExit, bool allowSameState);

        public void RevertToPrevious(TS_State previousState, bool doEnter, bool doExit, bool allowSameState);

        public bool IsSameState(TS_State s1, TS_State s2);

        public void Start(bool doEnter);
        public void Run();
        public void Stop();
    }
}