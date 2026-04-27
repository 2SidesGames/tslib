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
    public abstract class ServiceBaseSo : ScriptableObject
    {
        // Stores registered components indexed by their concrete type
        private readonly Dictionary<Type, TSComponent> _components = new();

        // Currently registered controller for this service
        private TSController _controller;

        /// Registers a controller for this service.
        /// Only one controller can be registered at a time.
        /// </summary>
        /// <param name="controller">Controller instance to register.</param>
        public void RegisterController(TSController controller)
        {
            if (controller == null)
                throw new ArgumentNullException(nameof(controller));

            _controller = controller;
        }

        /// <summary>
        /// Returns the currently registered controller, or null if none is registered.
        /// </summary>
        public T GetController<T>() where T : TSController => (T)_controller;

        /// <summary>
        /// Unregisters the currently registered controller.
        /// </summary>
        public void UnregisterController() => _controller = null;

        /// <summary>
        /// Registers a component instance using its concrete runtime type as the key.
        /// If a component of the same type already exists, it is overwritten.
        /// </summary>
        /// <param name="component">Component instance to register.</param>
        public void RegisterComponent(TSComponent component)
        {
            if (_components == null)
                throw new ArgumentNullException(nameof(_components));

            if (component == null)
                throw new ArgumentNullException(nameof(component));

            _components[component.GetType()] = component;
        }

        /// <summary>
        /// Retrieves a registered component by type.
        /// Returns null if the component is not found or no components are registered.
        /// </summary>
        /// <typeparam name="T">Component type to retrieve.</typeparam>
        public T GetComponent<T>() where T : TSComponent
        {
            if (_components?.Count == 0) return null;

            return _components.TryGetValue(typeof(T), out var value)
                ? (T)value : null;
        }

        /// <summary>
        /// Unregisters a component by its type.
        /// </summary>
        /// <typeparam name="T">Component type to unregister.</typeparam>
        /// <returns>True if the component was removed; otherwise false.</returns>
        public bool UnregisterComponent<T>() where T : TSComponent
        {
            if (_components == null)
                throw new ArgumentNullException(nameof(_components));

            return _components.Remove(typeof(T));
        }

        /// <summary>
        /// Registers multiple components in a single call.
        /// Each component is registered using its concrete type.
        /// </summary>
        /// <param name="components">Array of components to register.</param>
        public void RegisterComponents(TSComponent[] components)
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
        public bool UnregisterComponents(TSComponent[] components)
        {
            if (_components == null)
                throw new ArgumentNullException(nameof(_components));

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

                bool removed = _components.Remove(component.GetType());
                if (!removed) allRemoved = false;
            }

            return allRemoved;
        }
    }
}
