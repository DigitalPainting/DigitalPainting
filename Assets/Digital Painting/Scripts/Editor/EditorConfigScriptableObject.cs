using System;
using UnityEditor;
using UnityEngine;
using wizardscode.plugin;

namespace wizardscode.digitalpainting
{
    public class EditorConfigScriptableObject : ScriptableObject
    {
        public static string LatestVersion = "0.0.18";

        [Header("Prefabs")]
        public DigitalPaintingManager ManagerPrefab;

        [Header("Informational")]
        public string ManagerName = "Digital Painting Manager";
        public string DocsIndexURL = "https://github.com/DigitalPainting/DigitalPainting/tree/master/Assets/Digital%20Painting/Docs";

        internal string version;
        string rootPath = "Assets/DigitalPainting/Assets/Digital Painting/";
        string managerPrefabPath = "Prefabs/DigitalPaintingManager.prefab";

        public void Init()
        {
            version = EditorConfigScriptableObject.LatestVersion;
            ManagerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(rootPath + managerPrefabPath).GetComponent<DigitalPaintingManager>();
        }

        /// <summary>
        /// Upgrade from an old config to a new one.
        /// </summary>
        /// <param name="oldConfig">The config we want to update from.</param>
        internal void Upgrade(EditorConfigScriptableObject oldConfig)
        {
            Init();
            Upgrade(oldConfig, "ManagerName");
            Upgrade(oldConfig, "ManagerPrefab");
            Upgrade(oldConfig, "FlyingPathfinderPrefab");
        }

        private void Upgrade(EditorConfigScriptableObject old, String propertyName)
        {
            // By default we just copy the old value to the new one if it exists and use the default if it doesn't
            System.Reflection.PropertyInfo propInfo = old.GetType().GetProperty(propertyName);
            if (propInfo != null)
            {
                this.GetType().GetProperty(propertyName).SetValue(this, propInfo.GetValue(old));
            }

        }
    }
}
