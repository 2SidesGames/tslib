using SGLib.Utility.Patterns.Scene.Contexts;

namespace SGLib.Utility.Management.Component.Capabilities
{
    public interface IBindable
    {
        public void BindContext(SceneCtx sceneCtx, AppCtx appCtx);
        public void BindComponents();
    }
}
