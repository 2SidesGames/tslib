using System;
using System.Collections.Generic;
using TSLib.Utility.Management.Component.Capabilities;
using UnityEngine;

namespace TSLib.Utility.Management.Service
{
    /// <summary>
    /// ScriptableObject-based service that acts as a registry
    /// for components and a single controller instance.
    /// </summary>
    public abstract class TS_Service_So : ScriptableObject
    {
        // Stores registered components indexed by their concrete type
        private readonly Dictionary<Type, TS_Component> components = new();

        // Currently registered controller for this service
        private TS_Controller controller;

        /// Registers a controller for this service.
        /// Only one controller can be registered at a time.
        /// </summary>
        /// <param name="controller">Controller instance to register.</param>
        public void RegisterController(TS_Controller controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            this.controller = controller;
        }

        /// <summary>
        /// Returns the currently registered controller, or null if none is registered.
        /// </summary>
        public T GetController<T>() where T : TS_Controller => (T)controller;

        /// <summary>
        /// Unregisters the currently registered controller.
        /// </summary>
        public void UnregisterController() => controller = null;

        /// <summary>
        /// Registers a component instance using its concrete runtime type as the key.
        /// If a component of the same type already exists, it is overwritten.
        /// </summary>
        /// <param name="component">Component instance to register.</param>
        public void RegisterComponent(TS_Component component)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (component == null)
                throw new ArgumentNullException(nameof(component));

            components[component.GetType()] = component;
        }

        /// <summary>
        /// Retrieves a registered component by type.
        /// Returns null if the component is not found or no components are registered.
        /// </summary>
        /// <typeparam name="T">Component type to retrieve.</typeparam>
        public T GetComponent<T>() where T : TS_Component
        {
            if (components?.Count == 0) return null;

            return components.TryGetValue(typeof(T), out var value)
                ? (T)value : null;
        }

        /// <summary>
        /// Unregisters a component by its type.
        /// </summary>
        /// <typeparam name="T">Component type to unregister.</typeparam>
        /// <returns>True if the component was removed; otherwise false.</returns>
        public bool UnregisterComponent<T>() where T : TS_Component
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            return components.Remove(typeof(T));
        }

        /// <summary>
        /// Registers multiple components in a single call.
        /// Each component is registered using its concrete type.
        /// </summary>
        /// <param name="components">Array of components to register.</param>
        public void RegisterComponents(TS_Component[] components)
        {
            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (components.Length == 0)
                throw new InvalidOperationException(
                    "(empty) There is no components registered.");

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];
                RegisterComponent(component);
            }
        }

        /// <summary>
        /// Unregisters multiple components in a single call.
        /// Each component is removed using its concrete type.
        /// </summary>
        /// <param name="components">Array of components to unregister.</param>
        /// <returns>
        /// True if all components were successfully removed;
        /// false if one or more components were not found.
        /// </returns>
        public bool UnregisterComponents(TS_Component[] components)
        {
            if (this.components == null)
                throw new ArgumentNullException(nameof(TS_Service_So.components));

            if (components == null)
                throw new ArgumentNullException(nameof(components));

            if (components.Length == 0)
                throw new InvalidOperationException(
                    "(empty) There is no components registered.");

            bool allRemoved = true;

            for (int i = 0; i < components.Length; i++)
            {
                var component = components[i];

                if (component == null)
                    throw new ArgumentNullException(nameof(component));

                bool removed = this.components.Remove(component.GetType());
                if (!removed) allRemoved = false;
            }

            return allRemoved;
        }
    }
}
