using UnityEngine;

namespace SGLib.Utility.Patterns.EventChannels.NonPrimitive
{
    [CreateAssetMenu(
        fileName = "SceneChannel",
        menuName = "Event Channels/NonPrimitive/Scene Channel"
    )]
    public class SceneChannel_So : SG_ChannelT1_So<UnityEngine.SceneManagement.Scene> { }
}

