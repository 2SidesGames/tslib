using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using TSLib.Utility.Management.Component.Capabilities;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

public class UtilityAIController : TS_Controller
{
    [SerializeField] private TS_UtilityAI[] standardActions;
    [SerializeField] private int bufferSize = 3; // number of bests to pick
    [SerializeField] private int maxBuckets = 10;

    [Header("Subscription events")]
    [SerializeField] private VoidChannel_So[] chooseNextActionEvents;
    [SerializeField] private VoidChannel_So[] stopEvents;

    private TS_UtilityAI best;
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

        bufferSize = Mathf.Max(1, bufferSize);
        maxBuckets = Mathf.Max(1, maxBuckets);

        utilities = new TS_UtilityAI[ComponentArray.Length];

        for (int i = 0; i < ComponentArray.Length; i++)
        {
            utilities[i] = (TS_UtilityAI)ComponentArray[i];
        }

        bucketDict = new Dictionary<int, List<TS_UtilityAI>>(maxBuckets);
        topBuffer = new TS_UtilityAI[bufferSize];

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
        best = null;
    }

    public void ChooseNextAction()
    {
        ChooseNextActionAsync().Forget();
    }

    private async UniTask ChooseNextActionAsync()
    {
        if (isExecuting) return;

        isExecuting = true;

        var cts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken);

        try
        {
            var selected = SelectNextUtility();

            if (selected == null)
            {
                // continues the current loop action
                if (best != null && best.Data.IsLoop) return;

                // choosing a random default action
                int randomIndex = UnityEngine.Random.Range(0, standardActions.Length);
                best = standardActions[randomIndex];
            }
            else
            {
                // cancel cts for loop action checker
                if (best.Data.IsLoop)
                {
                    executionCts?.Cancel();
                    executionCts?.Dispose();
                    executionCts = null;
                }

                // changes action
                best = selected;
            }

            // saving new cts
            executionCts = cts;

            await best.ExecuteActionAsync(executionCts.Token);
        }
        catch (OperationCanceledException) when (executionCts.IsCancellationRequested) { }
        catch (Exception exception) { Debug.LogException(exception, this); }
        finally
        {
            executionCts?.Dispose();
            executionCts = null;
            isExecuting = false;
        }
    }

    private TS_UtilityAI SelectNextUtility()
    {
        // 0 priority is the highest one
        for (int p = 0; p < maxBuckets; p++)
        {
            if (!bucketDict.TryGetValue(p, out List<TS_UtilityAI> bucket)) continue;

            foreach (var utility in bucket)
            {
                utility.UpdateActiveCondition();

                // saving the score update because it is not choosable
                if (!utility.IsActive) continue;

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
        Array.Clear(topBuffer, 0, bufferSize);

        int count = 0; // how many utilities added to the buffer

        foreach (var utility in bucket)
        {
            if (!utility.IsChoosable()) continue;

            float score = Mathf.Clamp01(utility.CurrentScore);

            for (int i = 0; i < bufferSize; i++)
            {
                var currentTop = topBuffer[i];

                // dismiss
                if (currentTop != null && score <= currentTop.CurrentScore) continue;

                // repositioning tops
                for (int j = bufferSize - 1; j > i; j--)
                {
                    if (currentTop == null) break; // just insert it
                    topBuffer[j] = topBuffer[j - 1];
                }

                topBuffer[i] = utility;

                if (count < bufferSize) count++;

                break; // next utility
            }
        }
        return count;
    }
}