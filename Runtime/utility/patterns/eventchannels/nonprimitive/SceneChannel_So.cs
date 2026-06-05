using UnityEngine;

namespace TSLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "SceneChannel",
        menuName = "Event Channels/NonPrimitive/Scene Channel"
    )]
    public class SceneChannel_So : TS_ChannelT1_So<UnityEngine.SceneManagement.Scene> { }
}

