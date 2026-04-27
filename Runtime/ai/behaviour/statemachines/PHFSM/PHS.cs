using System;
using System.Collections.Generic;
using TSLib.AI.Behaviour.StateMachines.HFSM;
using TSLib.Utility.Patterns.EventChannels.Primitive;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    public abstract class PHS : HierarchicalState
    {
        public int Priority { get; set; }
        public bool IsInterruptible { private get; set; }

        public List<Transition> TransitionList { get; private set; }
        public Transition SelfTransition { get; private set; }

        public bool EnterCondition { get; private set; } = false;
        public bool ExitCondition { get; private set; } = false;
        public VoidChannel_So OnEnterCondition { private get; set; }
        public VoidChannel_So OnExitCondition { private get; set; }

        private IComparer<Transition> _comparer;


        public void Configure(PHSConfig_So config, IComparer<Transition> comparer)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));

            Priority = config.Priority;
            IsInterruptible = config.IsInterruptible;

            OnEnter = config.OnEnter ? config.OnEnter : null;
            OnExit = config.OnExit ? config.OnExit : null;

            OnEnterCondition = config.OnEnterCondition ? config.OnEnterCondition : null;
            OnExitCondition = config.OnExitCondition ? config.OnExitCondition : null;

            SelfTransition = new Transition(this);
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));

            if (OnEnterCondition != null) OnEnterCondition.Subscribe(EnableEnter);
            if (OnExitCondition != null) OnExitCondition.Subscribe(EnableExit);
        }


        public sealed override void Enter()
        {
            base.Enter();
            EnterCondition = false;
            EnterLogic();
        }

        public sealed override void Execute(StateMachineBase stateMachine, float deltaTime)
        {
            ExecuteLogic(stateMachine, deltaTime);

            // Transition checker
            if (!ExitCondition && !IsInterruptible) return;

            for (int i = 0; i < TransitionList.Count; i++)
            {
                var nextTransition = TransitionList[i];
                if (nextTransition == null) continue;

                if (!nextTransition.EnterCondition()) continue;
                if (stateMachine.IsSameState(nextTransition.NextState, this)) continue;

                // can transition to a state with higher priority
                bool isInterruption = !ExitCondition && IsInterruptible;
                bool isHigherPriorityState = !stateMachine.IsSameState(GetHigherPriorityState(SelfTransition, nextTransition), this);

                if (isInterruption && !isHigherPriorityState) break;

                stateMachine.TransitionTo(nextTransition.NextState);

                break;
            }
        }

        public sealed override void Exit()
        {
            ExitCondition = false;
            ExitLogic();
            base.Exit();
        }

        public void SetTransitionList(List<Transition> transitionList)
        {
            if (transitionList == null) throw new ArgumentNullException(nameof(transitionList));
            if (transitionList.Count == 0) throw new InvalidOperationException("(empty) no transitions to set.");

            TransitionList = transitionList;
            TransitionList.Sort(_comparer);
        }

        public void UnsubscribeConditionEvents()
        {
            if (OnEnterCondition != null) OnEnterCondition.Unsubscribe(EnableEnter);
            if (OnExitCondition != null) OnExitCondition.Unsubscribe(EnableExit);
        }

        protected virtual void EnterLogic() { }
        protected virtual void ExecuteLogic(StateMachineBase stateMachine, float deltaTime) { }
        protected virtual void ExitLogic() { }

        protected virtual void EnableEnter() => EnterCondition = true;
        protected virtual void EnableExit() => ExitCondition = true;
        protected virtual void DisableEnter() => EnterCondition = false;
        protected virtual void DisableExit() => ExitCondition = false;

        private PHS GetHigherPriorityState(Transition t1, Transition t2)
        {
            return _comparer.Compare(t1, t2) <= 0 ? t1.NextState : t2.NextState;
        }
    }
}
