using System;
using System.Collections.Generic;
using TSLib.Utility.Managers.Generators;
using TSLib.Utility.Patterns.Scene.Contexts;
using UnityEngine;

namespace TSLib.Utility.Management.Component.Capabilities
{
    public abstract class TS_Controller : TS_Component, IRegistrable
    {
        [SerializeField] protected TS_Component[] ComponentArray;
        protected Dictionary<Type, TS_Component> ComponentDict;

        public int Id { get; private set; }

        public override void Initialize()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            ComponentDict = new Dictionary<Type, TS_Component>(ComponentArray.Length);

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.Initialize();

                ComponentDict[component.GetType()] = component;
            }

            // set the unique Id for this controller
            Id = IdGenerator.GenerateId();
        }

        public override void BindContext(SceneCtx sceneCtx, AppCtx appCtx)
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            base.BindContext(sceneCtx, appCtx);

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.BindContext(sceneCtx, appCtx);
            }
        }

        public override void BindComponents()
        {
            if (ComponentArray == null) throw new InvalidOperationException(
                "(missing) components storage uninitialized.");

            for (int i = 0; i < ComponentArray.Length; i++)
            {
                var component = ComponentArray[i];
                if (component == null) continue;
                component.BindComponents();
            }
        }

        public virtual void Register() { }
        public virtual void UnRegister() { }

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

        public T GetTSComponent<T>() where T : TS_Component
        {
            if (ComponentDict == null) throw new InvalidOperationException(
                    "(missing) components dictionary uninitialized.");

            if (ComponentDict.Count == 0) return null;

            return ComponentDict.TryGetValue(typeof(T), out var component)
                ? (T)component : null;
        }

        public T RequireTSComponent<T>() where T : TS_Component
        {
            var component = GetTSComponent<T>();

            if (component == null) throw new InvalidOperationException(
                    "(missing) component not found.");

            return component;
        }
    }
}