using TSLib.Utility.Patterns.Scene.Contexts;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public interface IBindable
    {
        public void BindContext(SceneCtx sceneCtx, AppCtx appCtx);
        public void BindComponents();
    }
}
