using System;
using System.Collections.Generic;
using TSLib.Utility.Management.Component.Capabilities;
using UnityEngine;

namespace TSLib.Utility.Patterns.Scene.Loading
{
    public abstract class TS_SceneContainer : MonoBehaviour
    {
        [SerializeField] protected TS_Controller[] PrefabArray;
        protected Dictionary<Type, TS_Controller> PrefabDict;

        protected void CreateSceneContainer()
        {
            PrefabDict = new(PrefabArray.Length);

            for (int i = 0; i < PrefabArray.Length; i++)
            {
                var controller = PrefabArray[i];
                if (controller == null) continue;

                PrefabDict[controller.GetType()] = controller;
            }
        }

        protected T GetPrefab<T>() where T : TS_Controller
        {
            if (PrefabDict == null) throw new InvalidOperationException(
                    "(missing) prefabs dictionary uninitialized.");

            if (PrefabDict.Count == 0) return null;

            return PrefabDict.TryGetValue(typeof(T), out var prefab)
                ? (T)prefab : null;
        }

        protected T RequirePrefab<T>() where T : TS_Controller
        {
            var prefab = GetPrefab<T>();

            if (prefab == null) throw new InvalidOperationException(
                    "(missing) prefab not found.");

            return prefab;
        }
    }
}