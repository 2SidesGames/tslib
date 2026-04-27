using System;
using TSLib.Utility.Patterns.EventChannels.Primitive;
using UnityEngine;

namespace TSLib.AI.Behaviour.StateMachines.PHFSM
{
    [CreateAssetMenu(
    fileName = "PHSConfig",
    menuName = "Scriptable Objects/HFSM/PHSConfig"
)]
    public class PHSConfig_So : StateConfig_So
    {
        [field: SerializeField] public int Priority { get; private set; }
        [field: SerializeField] public bool IsInterruptible { get; private set; }

        [field: SerializeField] public VoidChannel_So OnEnterCondition { get; private set; }
        [field: SerializeField] public VoidChannel_So OnExitCondition { get; private set; }
    }
}
