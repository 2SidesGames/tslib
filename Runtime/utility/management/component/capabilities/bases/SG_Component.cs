using System.Collections.Generic;
using SGLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace SGLib.Utility.Management.Component.Capabilities
{
    public abstract class SG_Component : MonoBehaviour, IComponent
    {
        protected SceneCtx SceneCtx;
        protected AppCtx AppCtx;

        public virtual void Initialize() { }
        public virtual void BindContext(SceneCtx sceneCtx, AppCtx appCtx)
        {
            SceneCtx = sceneCtx;
            AppCtx = appCtx;
        }
        public virtual void BindComponents() { }
        public virtual void Configure() { }
        public virtual void Activate() { }
        public virtual void Deactivate() { }
        public virtual void Deconfigure() { }
    }
}
