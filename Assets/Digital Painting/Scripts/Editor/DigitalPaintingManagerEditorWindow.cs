using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.plugin;

namespace wizardscode.editor
{
    public class DigitalPaintingManagerEditorWindow : EditorWindow
    {
        private DigitalPaintingManager manager;
        int selectedTab = 0;
        private GUIStyle m_LinkStyle;
        private EditorConfigScriptableObject m_config;
        private string defaultConfigSavePath = "Assets/Digital Painting Editor Config.asset";

        Dictionary<string, List<AbstractPluginDefinition>> enabledPluginsCache = new Dictionary<string, List<AbstractPluginDefinition>>();
        Dictionary<string, List<AbstractPluginDefinition>> availablePluginsCache = new Dictionary<string, List<AbstractPluginDefinition>>();
        Dictionary<string, List<AbstractPluginDefinition>> supportedPluginsCache = new Dictionary<string, List<AbstractPluginDefinition>>();
        private float frequencyOfPluginRefresh = 5;
        private DateTime timeOfNextPluginRefresh = DateTime.Now;
        private string configAssetPath = "Assets/Digital Painting Editor Config.asset";

        private void OnEnable()
        {
            titleContent = new GUIContent("Digital Painting");
        }

        [MenuItem("Window/Wizards Code/Open Digital Painting Manager")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(DigitalPaintingManagerEditorWindow));
        }

        void OnGUI()
        {
            if (Application.isPlaying)
            {
                IsPlayingGUI();
            }
            else
            {
                GameObject go = GameObject.Find(Config.ManagerName);
                if (go)
                {
                    manager = go.GetComponent<DigitalPaintingManager>();
                }

                selectedTab = GUILayout.Toolbar(selectedTab, new string[] { "Standard", "Advanced", "Experimental", "More..." });
                switch (selectedTab)
                {
                    case 0:
                        StandardTabGUI();
                        break;
                    case 1:
                        NotImplementedTabGUI();
                        break;
                    case 2:
                        ExperimentalTabGUI();
                        break;
                    case 3:
                        MoreTabGUI();
                        break;
                }
            }
        }

        private void StandardTabGUI()
        {
            if (!manager)
            {
                if (GUILayout.Button("Add Digital Painting Manager"))
                {
                    AddDigitalPainting();
                }
            }
            else
            {
                PluginSelectionGUI<AbstractDayNightPluginDefinition>("Day Night");
                PluginSelectionGUI<AbstractWeatherPluginDefinition>("Weather");
            }
        }

        private void AddDigitalPainting()
        {
            // Parent Game Object
            GameObject parent = new GameObject();
            parent.name = "Wizards Code";

            // Digital Painting Manager
            manager = Instantiate(Config.ManagerPrefab, parent.transform);
            manager.name = Config.ManagerName;

            // Flying Pathfinding
            Octree octree = Instantiate(Config.FlyingPathfinderPrefab, parent.transform);
            octree.FitToTerrain(Terrain.activeTerrain);
        }

        private static void IsPlayingGUI()
        {
            GUILayout.Label("In play mode.");
        }

        private void MoreTabGUI()
        {
            if (LinkLabel("Latest Documentation on GitHub"))
            {
                Application.OpenURL(Config.DocsIndexURL);
            }

            GUILayout.FlexibleSpace();

            GUILayout.Label("Editor Version: " + Config.version);
        }

        private void ExperimentalTabGUI()
        {
            EditorGUILayout.HelpBox("The features on this tab are experimental and in development. They may or may not work. Play with them if you wish, but back up your project first.", MessageType.Warning);
        }

        /// <summary>
        /// Find all the plugins of a a given type known to the system and display
        /// buttons to set them up, disable them or find out more information.
        /// </summary>
        /// <typeparam name="T">Plugin definition type</typeparam>
        private void PluginSelectionGUI<T>(string name) where T : AbstractPluginDefinition
        {
            List<AbstractPluginDefinition> availablePlugins = new List<AbstractPluginDefinition>();
            List<AbstractPluginDefinition> enabledPlugins = new List<AbstractPluginDefinition>();
            List<AbstractPluginDefinition> supportedPlugins = new List<AbstractPluginDefinition>();

            if (DateTime.Now >= timeOfNextPluginRefresh || !availablePluginsCache.TryGetValue(name, out availablePlugins) || !supportedPluginsCache.TryGetValue(name, out supportedPlugins))
            {
                availablePlugins = new List<AbstractPluginDefinition>();
                enabledPlugins = new List<AbstractPluginDefinition>();
                supportedPlugins = new List<AbstractPluginDefinition>();

                IEnumerable<Type> plugins = ReflectiveEnumerator.GetEnumerableOfType<T>();
                foreach (Type pluginType in plugins)
                {
                    AbstractPluginDefinition pluginDef = Activator.CreateInstance(pluginType) as AbstractPluginDefinition;
                    if (pluginDef.AvailableForUse)
                    {
                        AbstractPluginManager pluginManager = (AbstractPluginManager)manager.GetComponent(pluginDef.GetManagerType());
                        if (pluginManager)
                        {
                            if (pluginManager.m_pluginProfile.GetType().Name == pluginDef.GetProfileTypeName())
                            {
                                enabledPlugins.Add(pluginDef);
                            } else
                            {
                                availablePlugins.Add(pluginDef);
                            }
                        }
                        else
                        {
                            availablePlugins.Add(pluginDef);
                        }
                    }
                    else
                    {
                        supportedPlugins.Add(pluginDef);
                    }
                }

                enabledPluginsCache[name] = enabledPlugins;
                availablePluginsCache[name] = availablePlugins;
                supportedPluginsCache[name] = supportedPlugins;
                timeOfNextPluginRefresh = DateTime.Now.AddSeconds(frequencyOfPluginRefresh);
            }

            GUILayout.BeginVertical("Box");
            GUILayout.Label(name + " Plugins", EditorStyles.boldLabel);


            if (enabledPluginsCache[name].Count + availablePluginsCache[name].Count > 0)
            {
                GUILayout.BeginVertical("Box");

                if (enabledPluginsCache[name].Count > 0)
                {
                    GUILayout.Label("Currently Enabled");
                    for (int i = enabledPluginsCache[name].Count - 1; i >= 0; i--)
                    {
                        AbstractPluginDefinition defn = enabledPluginsCache[name][i];
                        if (GUILayout.Button("Disable " + defn.GetReadableName()))
                        {
                            DestroyImmediate(manager.gameObject.GetComponent(defn.GetManagerType()));
                            enabledPluginsCache[name].Remove(defn);
                            availablePluginsCache[name].Add(defn);
                        }
                    }
                }

                if (availablePluginsCache[name].Count > 0)
                {
                    GUILayout.Label("Available for use");
                    for (int i = availablePluginsCache[name].Count - 1; i >= 0; i--)
                    {
                        AbstractPluginDefinition defn = availablePluginsCache[name][i];
                        bool hasManager = manager.gameObject.GetComponent(defn.GetManagerType()) != null;
                        using (new EditorGUI.DisabledScope(hasManager))
                        {
                            if (GUILayout.Button("Enable " + defn.GetReadableName()))
                            {
                                manager.gameObject.AddComponent(defn.GetManagerType());
                                enabledPluginsCache[name].Add(defn);
                                availablePluginsCache[name].Remove(defn);
                            }
                        }
                    }
                }

                GUILayout.EndVertical();
            }

            if (supportedPluginsCache[name].Count > 0)
            {
                GUILayout.BeginVertical("Box");
                GUILayout.Label("Supported but not installed");
                foreach (AbstractPluginDefinition defn in supportedPluginsCache[name])
                {
                    if (GUILayout.Button("Learn more about " + defn.GetReadableName() + "... "))
                    {
                        Application.OpenURL(defn.GetURL());
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndVertical();
        }
        private void NotImplementedTabGUI()
        {
            string message = "This tab has no functionality at this time.";
            GUILayout.Label(message);
        }

        GUIStyle LinkStyle
        {
            get
            {
                if (m_LinkStyle == null)
                {
                    m_LinkStyle = new GUIStyle(EditorStyles.label);
                    m_LinkStyle.wordWrap = false;
                    // Match selection color which works nicely for both light and dark skins
                    m_LinkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
                    m_LinkStyle.stretchWidth = false;
                }
                return m_LinkStyle;
            }
        }

        public EditorConfigScriptableObject Config
        {
            get
            {
                if (!m_config)
                {
                    m_config = AssetDatabase.LoadAssetAtPath<EditorConfigScriptableObject>(defaultConfigSavePath);
                    if (!m_config)
                    {
                        m_config = ScriptableObject.CreateInstance("EditorConfigScriptableObject") as EditorConfigScriptableObject;
                        m_config.Init();
                        AssetDatabase.CreateAsset(m_config, configAssetPath);
                    }
                }

                if (m_config.version != EditorConfigScriptableObject.LatestVersion)
                {
                    EditorConfigScriptableObject oldConfig = m_config;
                    m_config.Upgrade(oldConfig);
                    EditorUtility.SetDirty(m_config);
                    AssetDatabase.SaveAssets();
                }

                return m_config;
            }
        }

        bool LinkLabel(String text)
        {
            return LinkLabel(new GUIContent(text));
        }

        bool LinkLabel(GUIContent label, params GUILayoutOption[] options)
        {
            var position = GUILayoutUtility.GetRect(label, LinkStyle, options);

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return GUI.Button(position, label, LinkStyle);
        }
    }
}