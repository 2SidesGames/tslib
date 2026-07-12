using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Management.Component.Capabilities;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

public abstract class TS_UtilityAI : TS_Component
{
    [field: SerializeField] public UtilityData_So Data { get; private set; }

    [Header("Trigger Events")]
    [SerializeField] private VoidChannel_So onChooseNextAction;
    [SerializeField] private VoidChannel_So onStarted;
    [SerializeField] private VoidChannel_So onEnded;

    public bool IsActive { get; protected set; }
    public float CurrentScore { get; protected set; }
    protected float LastActionTime;

    public virtual void UpdateActiveCondition() => IsActive = true;

    public virtual void UpdateScore()
    {
        ApplyRewards();
        ApplyPenalties();
    }

    public virtual async UniTask ExecuteActionAsync(CancellationToken ct)
    {
        if (onStarted != null) onStarted.TriggerEvent();

        try
        {
            if (Data.IsLoop)
            {
                TriggerChooseNextAction(ct).Forget();
                ExecuteAsync(ct).Forget();
            }
            else
            {
                await ExecuteAsync(ct);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested) { }
        catch (Exception ex) { Debug.LogException(ex); }
        finally
        {
            LastActionTime = Time.time * 1000;
            if (onEnded != null) onEnded.TriggerEvent();
            if (!Data.IsLoop) onChooseNextAction.TriggerEvent();
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

        while (!ct.IsCancellationRequested)
        {
            await UniTask.Delay(delay, cancellationToken: ct);
            onChooseNextAction.TriggerEvent();
        }
    }
}