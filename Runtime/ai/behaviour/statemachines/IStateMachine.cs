namespace SGLib.AI.Behaviour.StateMachines
{
    public interface IStateMachine
    {
        public void Execute(float deltaTime);

        public void TransitionTo(SG_State newState, bool doEnter, bool doExit, bool allowSameState);

        public void RevertToPrevious(SG_State previousState, bool doEnter, bool doExit, bool allowSameState);

        public bool IsSameState(SG_State s1, SG_State s2);

        public void Start(bool doEnter);
        public void Run();
        public void Stop();
    }
}