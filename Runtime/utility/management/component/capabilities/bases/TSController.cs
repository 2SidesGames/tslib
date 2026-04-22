using System;
using System.Collections.Generic;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public abstract class TSController : TSComponent, IRegistrable
    {
        [SerializeField] protected TSComponent[] ComponentArray;
        protected Dictionary<Type, TSComponent> ComponentDict;

        public virtual void Register(SceneCtx sceneCtx, AppCtx appCtx) { }
        protected virtual void Unregister() { }

        public override void Initialize()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            ComponentDict = new Dictionary<Type, TSComponent>(ComponentArray.Length);

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Initialize();

                ComponentDict[component.GetType()] = component;
            }
        }

        public override void Inject(SceneCtx sceneCtx, AppCtx appCtx)
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Inject(sceneCtx, appCtx);
            }
        }

        public override void Configure()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Configure();
            }
        }

        protected virtual void OnEnable()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Activate();
            }
        }

        protected virtual void OnDisable()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Deactivate();
            }
        }

        protected virtual void OnApplicationQuit()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Deconfigure();
            }
        }

        public T GetTSComponent<T>() where T : TSComponent
        {
            if (ComponentDict == null) throw new InvalidOperationException(
                    "(missing) components dictionary uninitialized.");

            if (ComponentDict.Count == 0) return null;

            return ComponentDict.TryGetValue(typeof(T), out var value)
                ? (T)value : null;
        }

        public T RequireTSComponent<T>() where T : TSComponent
        {
            var component = GetTSComponent<T>();

            if (component == null) throw new InvalidOperationException(
                    "(missing) component not found.");

            return component;
        }
    }
}