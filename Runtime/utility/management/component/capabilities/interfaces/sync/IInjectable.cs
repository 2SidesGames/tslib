using TSLib.Utility.Patterns.Scene.Contexts;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public interface IInjectable
    {
        public void Inject(SceneCtx sceneCtx, AppCtx appCtx);
    }
}
