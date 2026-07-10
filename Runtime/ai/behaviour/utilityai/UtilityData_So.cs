using UnityEngine;

[CreateAssetMenu(
    fileName = "Utility_Data",
    menuName = "Scriptable Objects/Utility AI/Data"
)]
public class UtilityData_So : ScriptableObject
{
    [field: SerializeField, Tooltip("Whether the action should loop. Default action must be a loop action.")]
    public bool IsLoop { get; private set; }

    [field: SerializeField, Range(0, 100000, order = 100),
    Tooltip("Delay in milliseconds for choosing next action triggers in loop actions.")]
    public int ChooseNextActionDelay { get; private set; } // TODO: usar como tiempo de release

    [field: SerializeField, Range(0, 10, order = 1), Tooltip("Lower value, higher priority.")]
    public int Priority { get; private set; }

    [field: SerializeField, Min(0), Tooltip("Cooldown before the utility action can be executed again, in milliseconds. " +
    "Zero cooldown means the action may be instantly interrupted.")]
    public int Cooldown { get; private set; }

    [field: SerializeField, Tooltip("The function which defines how to calculate the score.")]
    public AnimationCurve UtilityCurve { get; private set; }
}