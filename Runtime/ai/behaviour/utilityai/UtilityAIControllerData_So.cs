using UnityEngine;

namespace SGLib.AI.Behaviour.UtilityAI
{
    [CreateAssetMenu(
        fileName = "UtilityAIController_Data",
        menuName = "Scriptable Objects/Utility AI/Controller Data")]
    public class UtilityAIControllerData_So : ScriptableObject
    {
        [SerializeField, Min(1), Tooltip("Number of actions that can be selected (Podium size).")]
        private int bufferSize = 3;

        [SerializeField, Min(1), Tooltip("Maximum number of buckets (defines priorities).")]
        private int maxBuckets = 10;
        [SerializeField, Range(0f, 1f), Tooltip("Minimum score required for actions to be selected (inclusive).")]
        private float minScoreRequired = 0.5f;

        public int BufferSize => bufferSize;
        public int MaxBuckets => maxBuckets;
        public float MinScoreRequired => minScoreRequired;
    }
}
