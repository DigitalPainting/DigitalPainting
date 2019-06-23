using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using wizardscode.digitalpainting;
using wizardscode.extension;
using wizardscode.validation;

namespace wizardscode.plugin
{
    /// <summary>
    /// The abstract implementation of a plugin definition. All plugins should implement this class.
    /// </summary>
    public abstract class AbstractPluginDefinition : ScriptableObject
    {
        public enum PluginCategory {
            Agent,
            DayNightCycle,
            Weather,
            Terrain,
            Miscellaneous
        }
        
        public virtual bool MultipleAllowed
        {
            get { return false; }
        }

        /// <summary>
        /// Get the Type of the plugin manager, that is the Type of the MonoBehaviour that
        /// is used by the `DigitalPaintingManager` to manage the plugin.
        /// </summary>
        /// <returns></returns>
        public abstract Type GetManagerType();

        /// <summary>
        /// Get the name of the type of the profile for this plugin implementation. This allows
        /// us to check that a profile exists and also to check whether a plugin implementation
        /// is enabled in the scene.
        /// </summary>
        /// <returns></returns>
        public abstract String GetProfileTypeName();

        /// <summary>
        /// The name of a class that we know exists in the plugin implementation. This is used to verify that
        /// any dependent assets are installed. If the plugin is self-contained, that is it does not required
        /// another asset to function correctly then this can be set to null.
        /// </summary>
        /// <returns></returns>
        public abstract string GetPluginImplementationClassName();

        /// <summary>
        /// Get a human readable name for this type of plugin use in the UI.
        /// </summary>
        /// <returns></returns>
        public abstract PluginCategory GetCategory();

        /// <summary>
        /// Get a human readable name for use in the UI.
        /// </summary>
        /// <returns></returns>
        public abstract string GetReadableName();

        /// <summary>
        /// Get a URL from which the dependent asset can be retrieved. Normally this would be an asset store
        /// page, but it could be somewhere else, such as a GitHub repo. If the plugin is self-contained and
        /// does not depend on another asset this can be set to null.
        /// </summary>
        /// <returns>Either the URL for retrieving an asset this plugin depends upon or null if there
        /// is no such dependency.</returns>
        public abstract string GetURL();

        /// <summary>
        /// Tests to see if the plugin asset is present and ready for use. The default test
        /// is to see if the class named by `GetPluginImplmentationClassName` is available in the
        /// assembly. If no class is named then true will be returned.
        /// </summary>
        /// <returns>True if the plugin asset is present and can be used, otherwise false.</returns>
        public virtual bool AvailableForUse
        {
            get
            {
                string className = GetPluginImplementationClassName();
                if(className == null)
                {
                    // there is no dependency on external assets, so it's available
                    return true;
                }
                IEnumerable<Type> types;
                if (className.Contains('.'))
                {
                    types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                from type in assembly.GetTypes()
                                where type.FullName == className
                                select type);
                }
                else
                {
                    types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                             from type in assembly.GetTypes()
                             where type.Name == className
                             select type);
                }

                if (types != null && types.Count() != 0)
                {
                    if (types.Count<Type>() > 1)
                    {
                        Debug.LogWarning("The PluginImplementationClassName " + GetPluginImplementationClassName() + " occurs more than once in the Assembly. We should really be checking namespaces too.");
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private DigitalPaintingManager _dpManager;
        private DigitalPaintingManager DigitalPaintingManager
        {
            get
            {
                if (_dpManager == null)
                {
                    _dpManager = GameObject.FindObjectOfType<DigitalPaintingManager>();
                }
                return _dpManager;
            }
        }


        /// <summary>
        /// Enable the plugin in your scene.
        /// </summary>
        public virtual void Enable()
        {
            string suffix = "";
            if (MultipleAllowed)
            {
                suffix = " " + (DigitalPaintingManager.gameObject.GetComponentsInChildren(GetManagerType()).Count() + 1);
            }

            GameObject go = new GameObject(GetManagerType().Name.ToString().Prettify() + suffix);
            AbstractPluginManager manager = (AbstractPluginManager)go.AddComponent(GetManagerType());
            go.transform.SetParent(DigitalPaintingManager.gameObject.transform);

            MonoScript script = MonoScript.FromScriptableObject(this);
            string scriptPath = AssetDatabase.GetAssetPath(script);
            int lastIndex = scriptPath.LastIndexOf("Scripts");
            string fromPath = scriptPath.Substring("Assets/".Length, lastIndex - 7) + AssetDatabaseUtility.dataFolderName;
            string toPath = GetPathToScene();

            SetupDefaultSettings(manager, fromPath, toPath);
        }

        private void SetupDefaultSettings(AbstractPluginManager manager, string fromPath, string toPath)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            string sceneName = scene.name;

            if (!AssetDatabase.IsValidFolder(toPath + "/" + AssetDatabaseUtility.dataFolderName))
            {
                AssetDatabase.CreateFolder(toPath, AssetDatabaseUtility.dataFolderName);
            }

            string pluginFolder = fromPath.Split('/').First();
            string fullToPath = toPath + "/" + AssetDatabaseUtility.dataFolderName + "/" + pluginFolder;
            if (!AssetDatabase.IsValidFolder(fullToPath))
            {
                AssetDatabase.CreateFolder(toPath + "/" + AssetDatabaseUtility.dataFolderName, pluginFolder);
            }

            try
            {
                string[] fileEntries = Directory.GetFiles(Application.dataPath + "/" + fromPath);
                foreach (string fileName in fileEntries)
                {
                    string temp = fileName.Replace("\\", "/");
                    int index = temp.LastIndexOf("/");
                    string localPath = "Assets/" + fromPath;

                    if (index > 0)
                    {
                        localPath += temp.Substring(index);
                    }

                    UnityEngine.Object original = AssetDatabase.LoadAssetAtPath(localPath, typeof(ScriptableObject));
                    if (original != null)
                    {
                        string filename = Path.GetFileName(localPath);
                        filename = filename.Replace("_Default", "_" + sceneName);
                        if (AssetDatabase.GetMainAssetTypeAtPath(fullToPath + "/" + filename) == null)
                        {
                            AssetDatabase.CopyAsset(localPath, fullToPath + "/" + filename);
                        }
                    }

                    // if AbstractPluginProfile copy it into Profile

                    AbstractPluginProfile profile = (AbstractPluginProfile)AssetDatabase.LoadAssetAtPath(localPath, typeof(AbstractPluginProfile));
                    if (profile != null)
                    {
                        string filename = Path.GetFileName(localPath);
                        filename = filename.Replace("_Default", "_" + sceneName);
                        AssetDatabase.CopyAsset(localPath, toPath + "/" + AssetDatabaseUtility.dataFolderName + "/" + filename);

                        manager.Profile = profile;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Unable to copy plugin default settings. Digital Painting expects to find the default settings in " + fromPath + " see following exception for more information", this);
                Debug.LogException(e);
            }
        }

        private static string GetPathToScene()
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            string sceneName = scene.name;
            string path = scene.path.Substring(0, scene.path.Length - ("/" + sceneName + ".unity").Length);
            return path;
        }

        public virtual void Disable()
        {
            GameObject go = DigitalPaintingManager.gameObject.GetComponentInChildren(GetManagerType()).gameObject;
            DestroyImmediate(go);
        }
    }
}
