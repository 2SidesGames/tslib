using System;
using SGLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

namespace SGLib.AI.Behaviour.StateMachines
{
    [CreateAssetMenu(
    fileName = "StateConfig_So",
    menuName = "Scriptable Objects/StateMachine/State Config"
)]
    public class StateConfig_So : ScriptableObject
    {
        [field: SerializeField] public VoidChannel_So OnEnter { get; private set; }
        [field: SerializeField] public VoidChannel_So OnExit { get; private set; }
    }
}