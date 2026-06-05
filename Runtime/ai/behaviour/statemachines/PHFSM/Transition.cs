using System;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public class Transition
    {
        public TS_PrioritizedHierarchicalState NextState { get; private set; }

        public Transition() { }
        public Transition(TS_PrioritizedHierarchicalState nextState) => SetNextState(nextState);

        public void SetNextState(TS_PrioritizedHierarchicalState nextState)
        {
            if (NextState != null) throw new InvalidOperationException(
                "(invalid) NextState is already assigned.");

            NextState = nextState ?? throw new ArgumentNullException(nameof(nextState));
        }

        public bool EnterCondition()
        {
            if (NextState == null) throw new ArgumentNullException(nameof(NextState));
            return NextState.EnterCondition;
        }
    }
}
