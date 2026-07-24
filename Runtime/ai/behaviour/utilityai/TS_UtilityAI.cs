using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Debug.Logging;
using TSLib.Utility.Management.Component.Capabilities;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

namespace TSLib.AI.Behaviour.UtilityAI
{
    public abstract class TS_UtilityAI : TS_Component
    {
        [field: SerializeField] public UtilityData_So Data { get; private set; }

        [Header("Trigger Events")]
        [SerializeField] private VoidChannel_So onChooseNextAction;

        public bool IsActive { get; protected set; }
        public float CurrentScore { get; protected set; }
        protected float LastActionTime;

        public virtual void UpdateActiveCondition() => IsActive = true;

        public virtual void UpdateScore()
        {
            if (!IsActive) return;

            ApplyRewards();
            ApplyPenalties();
        }

        public virtual async UniTask ExecuteActionAsync(CancellationToken ct)
        {
            if (Data.IsLoop) TriggerChooseNextAction(ct).Forget();

            try
            {
                await ExecuteAsync(ct);
            }
            finally
            {
                LastActionTime = Time.time * 1000;
            }
        }

        public bool IsChoosable()
        {
            if (!IsActive) return false;

            // is cooldown completed?
            var currentTime = Time.time * 1000; // in ms
            return currentTime - LastActionTime > Data.Cooldown;
        }

        public void SetActive(bool active) => IsActive = active;

        protected virtual void ApplyPenalties() { }
        protected virtual void ApplyRewards() { }
        protected abstract UniTask ExecuteAsync(CancellationToken ct);

        private async UniTask TriggerChooseNextAction(CancellationToken ct)
        {
            var delay = Mathf.Max(Data.ChooseNextActionDelay, Data.Cooldown);

            while (true)
            {
                bool wasCancelled = await UniTask.Delay(delay, cancellationToken: ct).SuppressCancellationThrow();

                if (wasCancelled || ct.IsCancellationRequested) return;

                onChooseNextAction.TriggerEvent();
            }
        }
    }
}
