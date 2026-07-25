namespace SGLib.AI.Behaviour.StateMachines.PFSM
{
    /// <summary>
    /// Represents a preemptive state that can interrupt the current active state
    /// of the PFSM and take control when its preemption conditions are met.
    /// </summary>
    /// <remarks>
    /// <para>
    /// A <see cref="SG_PreemptiveState"/> is both a decision maker and a regular state.
    /// While it is not the current active state, its
    /// <see cref="EvaluatePreemption(Preemptive_FSM)"/> method is evaluated every update
    /// cycle to determine whether a transition into this state (or another state) should occur.
    /// </para>
    /// <para>
    /// When a preemptive state becomes the current active state, it fully participates in the
    /// normal state lifecycle: its <see cref="SG_State.Enter"/>,
    /// <see cref="SG_State.Execute"/>, and <see cref="SG_State.Exit"/> methods are
    /// invoked just like any other state.
    /// </para>
    /// <para>
    /// This pattern is intended for high-priority or interrupting behaviors that must be able
    /// to take control from any other state, such as forced actions, emergency states, or
    /// external interruptions.
    /// </para>
    /// </remarks>
    public abstract class SG_PreemptiveState : SG_State
    {
        /// <summary>
        /// Evaluates whether this state should preempt the currently active state.
        /// </summary>
        /// <param name="pfsm">The finite state machine evaluating the preemption.</param>
        public abstract void EvaluatePreemption(Preemptive_FSM pfsm);
    }
}