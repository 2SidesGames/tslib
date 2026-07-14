using UnityEngine;

[CreateAssetMenu(
    fileName = "UtilityAIController_Data",
    menuName = "Scriptable Objects/Utility AI/Controller Data"
)]
public class UtilityAIControllerData_So : ScriptableObject
{
    [SerializeField, Min(1)] private int bufferSize = 3; // podium size
    [SerializeField, Min(1)] private int maxBuckets = 10;
    [SerializeField, Range(0f, 1f)] private float minScoreRequired = 0.5f;

    public int BufferSize => bufferSize;
    public int MaxBuckets => maxBuckets;
    public float MinScoreRequired => minScoreRequired;
}