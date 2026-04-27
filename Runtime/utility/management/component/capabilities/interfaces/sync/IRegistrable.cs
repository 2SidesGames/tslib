using TSLib.Utility.Patterns.Scene.Contexts;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public interface IRegistrable
    {
        /// <summary>
        /// Registers the component with the
        /// relevant services.
        /// </summary>
        public void Register(SceneCtx sceneCtx, AppCtx appCtx);
    }
}

