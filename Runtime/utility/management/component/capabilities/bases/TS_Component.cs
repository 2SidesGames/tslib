using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public abstract class TS_Component : MonoBehaviour, IComponent
    {
        protected SceneCtx SceneCtx { get; set; }
        protected AppCtx AppCtx { get; set; }
        public virtual void Initialize() { }
        public virtual void Inject(SceneCtx sceneCtx, AppCtx appCtx) { }
        public virtual void Configure() { }
        public virtual void Activate() { }
        public virtual void Deactivate() { }
        public virtual void Deconfigure() { }
    }
}
