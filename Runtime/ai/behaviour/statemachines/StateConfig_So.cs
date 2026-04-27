using System;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

namespace TSLib.AI.Behaviour.StateMachines
{
    [CreateAssetMenu(
    fileName = "StateConfig_So",
    menuName = "Scriptable Objects/StateMachine/StateConfig_So"
)]
    public class StateConfig_So : ScriptableObject
    {
        [field: SerializeField] public VoidChannel_So OnEnter { get; private set; }
        [field: SerializeField] public VoidChannel_So OnExit { get; private set; }
    }
}