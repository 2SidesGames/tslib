using UnityEngine;

namespace TSLib.AI.Behaviour.UtilityAI
{
    [CreateAssetMenu(
        fileName = "Utility_Data",
        menuName = "Scriptable Objects/Utility AI/Data"
    )]
    public class UtilityData_So : ScriptableObject
    {
        [field: SerializeField, Tooltip("Whether the action should loop. Default actions must be loop actions.")]
        public bool IsLoop { get; private set; }

        [field: SerializeField, Range(0, 100000, order = 100),
        Tooltip("Delay in milliseconds for requesting choosing next action in loop actions.")]
        public int ChooseNextActionDelay { get; private set; } // TODO: usar como tiempo de release

        [field: SerializeField, Min(0), Tooltip("Lower value, higher priority.")]
        public int Priority { get; private set; }

        [field: SerializeField, Min(0), Tooltip("Cooldown before the utility action can be executed again, in milliseconds. " +
        "Zero cooldown means the action may be instantly selected again.")]
        public int Cooldown { get; private set; }

        [field: SerializeField, Tooltip("The function which defines how to calculate the score.")]
        public AnimationCurve UtilityCurve { get; private set; }
    }
}
