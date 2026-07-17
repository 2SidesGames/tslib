using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Management.Component.Capabilities;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

public class UtilityAIController : TS_Controller
{
    [SerializeField] private TS_UtilityAI[] standardActions;
    [SerializeField] private UtilityAIControllerData_So data;

    [Header("Trigger events")]
    [SerializeField] private VoidChannel_So[] actionStartedEvents;
    [SerializeField] private VoidChannel_So[] actionEndedEvents;

    [Header("Subscription events")]
    [SerializeField] private VoidChannel_So[] chooseNextActionEvents;
    [SerializeField] private VoidChannel_So[] stopEvents;

    private TS_UtilityAI currentAction;
    private TS_UtilityAI[] utilities;
    private TS_UtilityAI[] topBuffer;
    private Dictionary<int, List<TS_UtilityAI>> bucketDict;
    private bool isExecuting;
    private CancellationTokenSource executionCts;

    public override void Initialize()
    {
        if (standardActions == null || standardActions.Length == 0)
        {
            throw new NullReferenceException("(missing) At least one default action must be set.");
        }

        for (int i = 0; i < standardActions.Length; i++)
        {
            var standard = standardActions[i];
            if (!standard.Data.IsLoop)
            {
                throw new InvalidDataException("Default Utility AI must be a loop action.");
            }
        }

        utilities = new TS_UtilityAI[ComponentArray.Length];

        for (int i = 0; i < ComponentArray.Length; i++)
        {
            utilities[i] = (TS_UtilityAI)ComponentArray[i];
        }

        bucketDict = new Dictionary<int, List<TS_UtilityAI>>(data.MaxBuckets);
        topBuffer = new TS_UtilityAI[data.BufferSize];

        for (int i = 0; i < utilities.Length; i++)
        {
            TS_UtilityAI utility = utilities[i];

            if (utility == null) continue;

            int priority = utility.Data.Priority;

            if (bucketDict.TryGetValue(priority, out List<TS_UtilityAI> bucket))
            {
                bucket.Add(utility);
            }
            else
            {
                bucketDict.Add(priority, new List<TS_UtilityAI> { utility });
            }
        }

        base.Initialize();
    }

    public override void Activate()
    {
        if (chooseNextActionEvents != null)
        {
            for (int i = 0; i < chooseNextActionEvents.Length; i++)
            {
                var onChooseNextAction = chooseNextActionEvents[i];
                if (onChooseNextAction == null) continue;

                onChooseNextAction.Subscribe(ChooseNextAction);
            }
        }

        if (stopEvents != null)
        {
            for (int i = 0; i < stopEvents.Length; i++)
            {
                var onStop = stopEvents[i];
                if (onStop == null) continue;

                onStop.Subscribe(Stop);
            }
        }

        base.Activate();
    }

    public override void Deactivate()
    {
        if (chooseNextActionEvents != null)
        {
            for (int i = 0; i < chooseNextActionEvents.Length; i++)
            {
                var onChooseNextAction = chooseNextActionEvents[i];
                if (onChooseNextAction == null) continue;

                onChooseNextAction.Unsubscribe(ChooseNextAction);
            }
        }

        if (stopEvents != null)
        {
            for (int i = 0; i < stopEvents.Length; i++)
            {
                var onStop = stopEvents[i];
                if (onStop == null) continue;

                onStop.Unsubscribe(Stop);
            }
        }

        base.Deactivate();
    }

    public void Stop()
    {
        executionCts?.Cancel();
        currentAction = null;
    }

    public void ChooseNextAction()
    {
        ChooseNextActionAsync().Forget();
    }

    private async UniTask ChooseNextActionAsync()
    {
        if (isExecuting) return;

        var selected = SelectNextUtility();

        if (selected == null)
        {
            // continues the current loop action
            if (currentAction != null && currentAction.Data.IsLoop) return;

            // choosing a random default action
            int randomIndex = UnityEngine.Random.Range(0, standardActions.Length);
            currentAction = standardActions[randomIndex];
        }
        else
        {
            if (currentAction != null && currentAction.Data.IsLoop)
            {
                // continues the current loop action
                if (ReferenceEquals(selected, currentAction)) return;

                // cancels and remove previous cts of loop action checker
                executionCts?.Cancel();
                executionCts?.Dispose();
                executionCts = null;
            }

            // changes action
            currentAction = selected;
        }

        isExecuting = true;

        // new cts
        var cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);
        executionCts = cts;

        // notifies start
        TriggerEvents(actionStartedEvents);

        try
        {
            await currentAction.ExecuteActionAsync(cts.Token);
        }
        finally
        {
            isExecuting = false;

            if (!currentAction.Data.IsLoop || cts.IsCancellationRequested)
            {
                cts.Dispose();

                if (ReferenceEquals(executionCts, cts)) { executionCts = null; }

                ChooseNextAction(); // starts again after finite action
            }

            // notifies ending
            TriggerEvents(actionEndedEvents);
        }
    }

    private TS_UtilityAI SelectNextUtility()
    {
        // 0 priority is the highest one
        for (int p = 0; p < data.MaxBuckets; p++)
        {
            if (!bucketDict.TryGetValue(p, out List<TS_UtilityAI> bucket)) continue;

            foreach (var utility in bucket)
            {
                utility.UpdateActiveCondition();
                utility.UpdateScore();
            }

            int count = InsertTopUtilities(bucket);

            if (count == 0) continue; // next bucket

            int randomIndex = UnityEngine.Random.Range(0, count);
            return topBuffer[randomIndex];
        }
        return null;
    }

    private int InsertTopUtilities(List<TS_UtilityAI> bucket)
    {
        int size = data.BufferSize;
        Array.Clear(topBuffer, 0, size);

        int count = 0; // how many utilities added to the buffer

        foreach (var candidate in bucket)
        {
            if (!candidate.IsChoosable()) continue;

            float score = Mathf.Clamp01(candidate.CurrentScore);

            // next utility
            if (score <= data.MinScoreRequired) continue;

            for (int i = 0; i < size; i++)
            {
                var currentTop = topBuffer[i];

                if (currentTop != null)
                {
                    // dismiss
                    if (score <= currentTop.CurrentScore) continue;

                    // repositioning tops
                    for (int j = size - 1; j > i; j--)
                    {
                        topBuffer[j] = topBuffer[j - 1];
                    }
                }

                topBuffer[i] = candidate;

                if (count < size) count++;

                break; // next utility
            }
        }
        return count;
    }

    private void TriggerEvents(VoidChannel_So[] events)
    {
        if (events == null) return;

        for (int i = 0; i < events.Length; i++)
        {
            var e = events[i];
            if (e == null) continue;

            e.TriggerEvent();
        }
    }
}