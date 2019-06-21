using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using wizardscode.digitalpainting;
using wizardscode.extension;
using wizardscode.plugin;
using wizardscode.validation;

namespace wizardscode.editor
{
    public class DigitalPaintingManagerEditorWindow : EditorWindow
    {
        private DigitalPaintingManager manager;

        public List<string> ignoredTests = new List<string>();

        private static Vector2 scrollPosition = Vector2.zero;
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
        
        protected static ValidationResultCollection Validations = new ValidationResultCollection();

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
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
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

                selectedTab = GUILayout.Toolbar(selectedTab, new string[] { "Status", "Standard", "Advanced", "Experimental", "More..." });


                if (!manager)
                {
                    if (EditorSceneManager.GetActiveScene().name.Length == 0)
                    {
                        EditorGUILayout.LabelField("Save the scene if you want to add the Digital Painting Manager.");
                    }
                    else
                    {
                        if (GUILayout.Button("Add Digital Painting Manager"))
                        {
                            AddDigitalPainting();
                        }
                    }
                }
                else
                {
                    switch (selectedTab)
                    {
                        case 0:
                            ValidationResultsGUI();
                            break;
                        case 1:
                            StandardTabGUI();
                            break;
                        case 2:
                            NotImplementedTabGUI();
                            break;
                        case 3:
                            ExperimentalTabGUI();
                            break;
                        case 4:
                            MoreTabGUI();
                            break;
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        static bool showErrors = true;
        static bool showIgnoredErrors = false;
        static bool showWarnings = true;
        private bool showIgnoredWarnings = false;
        static bool showOKs = true;
        private bool showIgnoredOKs = false;

        public void ShowValidationResults(ValidationResultCollection messages)
        {
            if (messages.Count == 0)
            {
                return;
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            List<ValidationResult> ignored = new List<ValidationResult>();
            List<ValidationResult> msgs = messages.ErrorList;
            showErrors = EditorGUILayout.Foldout(showErrors, "Errors: " + msgs.Count());
            if (showErrors)
            {
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in msgs)
                {
                    if (!ignoredTests.Contains(msg.name))
                    {
                        ValidationResultGUI(msg);
                    }
                    else
                    {
                        ignored.Add(msg);
                    }
                }

                showIgnoredErrors = EditorGUILayout.Foldout(showIgnoredErrors, "Ignored Errors: " + ignored.Count());
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in ignored)
                {
                    if (showIgnoredErrors)
                    {
                        ValidationResultGUI(msg, true);
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            ignored = new List<ValidationResult>();
            msgs = messages.WarningList;
            showWarnings = EditorGUILayout.Foldout(showWarnings, "Warnings: " + msgs.Count());
            if (showWarnings)
            {
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in msgs)
                {
                    if (!ignoredTests.Contains(msg.name))
                    {
                        ValidationResultGUI(msg);
                    } else
                    {
                        ignored.Add(msg);
                    }
                }

                showIgnoredWarnings = EditorGUILayout.Foldout(showIgnoredWarnings, "Ignored Warnings: " + ignored.Count());
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in ignored)
                {
                    if (showIgnoredWarnings)
                    {
                        ValidationResultGUI(msg, true);
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            ignored = new List<ValidationResult>();
            msgs = messages.OKList;
            showOKs = EditorGUILayout.Foldout(showOKs, "OK: " + msgs.Count());
            if (showOKs)
            {
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in msgs)
                {
                    if (!ignoredTests.Contains(msg.name))
                    {
                        ValidationResultGUI(msg);
                    }
                    else
                    {
                        ignored.Add(msg);
                    }
                }

                showIgnoredOKs = EditorGUILayout.Foldout(showIgnoredWarnings, "Ignored OK: " + ignored.Count());
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in ignored)
                {
                    if (showIgnoredOKs)
                    {
                        ValidationResultGUI(msg, true);
                    }
                }
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        private void ValidationResultGUI(ValidationResult result, bool isIgnored = false)
        {
            EditorGUILayout.BeginVertical("Box");
                MessageType messageType;
                switch (result.impact) {
                    case ValidationResult.Level.Error:
                        messageType = MessageType.Error;
                        break;
                    case ValidationResult.Level.Warning:
                        messageType = MessageType.Warning;
                        break;
                    default:
                        messageType = MessageType.Info;
                        break;
                }

                EditorGUILayout.BeginHorizontal();

                    string helpMsg;
                    if (result.Message != null)
                    {
                        helpMsg = result.Message;
                    } else
                    {
                        helpMsg = result.name;
                    }

                    EditorGUILayout.HelpBox(helpMsg, messageType, true);
            
                    EditorGUILayout.BeginVertical();
                        if (result.Callbacks != null)
                        {
                            foreach (ResolutionCallback callback in result.Callbacks)
                            {
                                if (GUILayout.Button(callback.Label))
                                {
                                    Validations.Remove(result.name);
                                    callback.ProfileCallback();
                                }
                            }
                        }

                        if (result.impact != ValidationResult.Level.OK)
                        {
                            if (!isIgnored)
                            {
                                if (GUILayout.Button("Ignore"))
                                {
                                    ignoredTests.Add(result.name);
                                }
                            }
                            else
                            {
                                if (GUILayout.Button("Do Not Ignore"))
                                {
                                    ignoredTests.Remove(result.name);
                                }
                            }
                        }

                    EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                if (result.ReportingTest != null)
                {
                    string tests = "";
                    foreach (string test in result.ReportingTest)
                    {
                        if (tests.Length > 0)
                        {
                            tests += ", " + test.Prettify();
                        } else
                        {
                            tests = test.Prettify();
                        }
                    }
                    EditorGUILayout.LabelField("Reported by: " + tests);
                }

            EditorGUILayout.EndVertical();
        }

        private void ValidationResultsGUI()
        {
            Validate();

            int okCount = Validations.Count;
            int warningCount = Validations.CountWarning;
            int errorCount = Validations.CountError;
            string title = "Validation (" + errorCount + " Errors, " + warningCount + " warnings, " + okCount + " OK)";

            showMainValidation = EditorGUILayout.Foldout(showMainValidation, title);

            if (Validations.Count > 0)
            {   
                if (showMainValidation || errorCount > 0)
                {
                    ShowValidationResults(Validations);
                }
            }
        }
        
        /// <summary>
        /// Test to see if the Digital Painting is setup correctly in the current scene. 
        /// Tests are discovered automatically as long as they implement the SingletonValidationTest
        /// Results of all 
        /// the validation tests are stored in an internal cache.
        /// </summary>
        //were found.</returns>
        public virtual void Validate()
        {
            timeToNextValidation -= Time.deltaTime;
            if (timeToNextValidation <= 0)
            {
                Validations = new ValidationResultCollection();
                List<Type> types = GetValidationTestsToRun();

                foreach (Type type in types)
                {
                    var test = Activator.CreateInstance(type);
                    MethodInfo method = type.GetMethod("Validate");
                    ValidationResultCollection results = (ValidationResultCollection)method.Invoke(test, new object[] { type });

                    Validations.AddOrUpdateAll(results);
                }
                timeToNextValidation = frequencyOfValidation;
            }
        }

        private static List<Type> GetValidationTestsToRun()
        {
            Type baseType = typeof(ValidationTest<>);

            IEnumerable<Type> candidates = from x in Assembly.GetAssembly(baseType).GetTypes()
                                           where !x.IsAbstract && !x.IsInterface && x != baseType
                                           select x;

            List<Type> types = new List<Type>();
            foreach (Type candidate in candidates)
            {
                if (ReflectionHelper.IsAssignableToGenericType(candidate, baseType))
                {
                    types.Add(candidate);
                }
            }

            return types;
        }

        private void StandardTabGUI()
        {
            ShowStatusSummaryGUI();

            IEnumerable<Type> types = GetAllPluginDefinitions();

            foreach (Type type in types)
            {
                PluginSelectionGUI(type);
            }
        }

        /// <summary>
        /// Shows a small panel with a summary of errors. Intended for inclusion at the top of
        /// each GUI panel.
        /// </summary>
        private void ShowStatusSummaryGUI()
        {
            Validate();

            int totalOKCount = Validations.CountOK;
            int notIgnoredOKCount = Validations.GetOKs(ignoredTests).Count();

            int totalWarningCount = Validations.CountWarning;
            int notIgnoredWarningCount = Validations.GetWarnings(ignoredTests).Count();

            int totalErrorCount = Validations.CountError;
            int notIgnoredErrorCount = Validations.GetErrors(ignoredTests).Count();

            string title = "Errors: " + notIgnoredErrorCount + " + (" + (totalErrorCount - notIgnoredErrorCount) + " ignored)\n";
            title += "Warnings: " + notIgnoredWarningCount + " + (" + (totalWarningCount - notIgnoredWarningCount) + " ignored)\n";
            title += "OK: " + totalOKCount;

            MessageType type = MessageType.Info;

            ValidationResult result;
            if(notIgnoredErrorCount > 0)
            {
                type = MessageType.Error;
                result = Validations.GetHighestPriorityErrorOrWarning(ignoredTests);
            } else if (notIgnoredWarningCount > 0)
            {
                type = MessageType.Warning;
                result = Validations.GetHighestPriorityErrorOrWarning(ignoredTests);
            } else
            {
                title = "Everything looks to be set up correctly.\n";
                title += (totalErrorCount - notIgnoredErrorCount) + " ignored errors.\n";
                title += (totalWarningCount - notIgnoredWarningCount) + " ignored warnings.\n";
                result = null;
            }
            
            EditorGUILayout.HelpBox(title, type);

            if (result != null)
            {
                ValidationResultGUI(result);
            }
        }

        /// <summary>
        /// Get an Enumeration of all available plugins types known to the system.
        /// This is a complete, unsorted list. However, we also cache the list of plugins 
        /// by status. This cache is updated by this method:
        /// 
        /// `avaiallbePluginsCache` - Available to be used, but not enabled in the scene.
        /// `enabledPluginsCache` - Available to be used and enabled in the scene.
        /// `supportedPluginsCache` - Known plugins that are not yet available for use (likely has a dependency on a missing asset)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Type> GetAllPluginDefinitions()
        {
            IEnumerable<Type> types = ReflectionHelper.GetClassesWithBaseType<AbstractPluginDefinition>();

            if (DateTime.Now >= timeOfNextPluginRefresh)
            {
                foreach (Type type in types)
                {
                    UpdatePluginStatus(type);
                }
                timeOfNextPluginRefresh = DateTime.Now.AddSeconds(frequencyOfPluginRefresh);
            }

            return types;
        }

        /// <summary>
        /// Adds the required Digital Painting assets and its dependencies.
        /// </summary>
        private void AddDigitalPainting()
        {
            // Parent Game Object
            // GameObject parent = new GameObject();
            // parent.name = "Wizards Code";

            // Digital Painting Manager
            manager = Instantiate(Config.ManagerPrefab);
            manager.name = Config.ManagerName;
        }

        /// <summary>
        /// Install all dependencies of Digital Painting that are not already present.
        /// </summary>
        private static void InstallDependencies()
        {
            string[] requiredPackages = { "Cinemachine", "TextMesh Pro" };
            foreach (string package in requiredPackages)
            {
                AddPackage(package, req =>
                {
                    if (req.Status == StatusCode.Success)
                    {
                        _request = null;
                    }
                });
            }
        }

        static Request _request;
        static Action<Request> _callback;
        private bool showMainValidation;
        private float timeToNextValidation = 0;
        private float frequencyOfValidation = 1;

        public static void AddPackage(string packageId, Action<Request> callback = null)
        {
            _request = Client.Add(packageId);
            _callback = callback;
            EditorUtility.DisplayProgressBar("Add Package", "Cloning " + packageId, 0.5f);
            EditorApplication.update += UpdateAddPackageRequest;
        }

        static void UpdateAddPackageRequest()
        {
            if (_request.Status != StatusCode.InProgress)
            {
                if (_request.Status == StatusCode.Failure)
                {
                    Debug.LogErrorFormat("Error: {0} ({1})", _request.Error.message, _request.Error.errorCode);
                }

                EditorApplication.update -= UpdateAddPackageRequest;
                EditorUtility.ClearProgressBar();
                if (_callback != null)
                {
                    _callback(_request);
                }
                _request = null;
                return;
            }
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
            ShowStatusSummaryGUI();
        }

        /// <summary>
        /// Find all the plugins of a a given type known to the system and display
        /// a UI for working with them.
        /// </summary>
        /// <param name="pluginDefinitionType">The type of plugin to display in the GUI</param>
        private void PluginSelectionGUI (Type pluginDefinitionType)
        {
            GUILayout.BeginVertical("Box");
            string categoryName = pluginDefinitionType.Name.Prettify();
            categoryName = categoryName.Substring(categoryName.IndexOf(' '));
            categoryName = categoryName.Substring(0, categoryName.Length - " Plugin Definition".Length);

            GUILayout.Label(categoryName + " Plugins", EditorStyles.boldLabel);

            if (enabledPluginsCache[pluginDefinitionType.Name].Count + availablePluginsCache[pluginDefinitionType.Name].Count > 0)
            {
                GUILayout.BeginVertical("Box");
                EnabledPluginsGUI(pluginDefinitionType);
                AvailablePluginsGUI(pluginDefinitionType);
                GUILayout.EndVertical();
            }

            SuportedPluginsGUI(pluginDefinitionType);
            GUILayout.EndVertical();
        }

        private void SuportedPluginsGUI(Type pluginDefinitionType)
        {
            if (supportedPluginsCache[pluginDefinitionType.Name].Count > 0)
            {
                GUILayout.BeginVertical("Box");
                GUILayout.Label("Supported but not installed");
                foreach (AbstractPluginDefinition defn in supportedPluginsCache[pluginDefinitionType.Name])
                {
                    if (GUILayout.Button("Learn more about " + defn.GetReadableName() + "... "))
                    {
                        Application.OpenURL(defn.GetURL());
                    }
                }
                GUILayout.EndVertical();
            }
        }

        private void AvailablePluginsGUI(Type pluginDefinitionType)
        {
            if (availablePluginsCache[pluginDefinitionType.Name].Count > 0)
            {
                GUILayout.Label("Available for use");
                for (int i = availablePluginsCache[pluginDefinitionType.Name].Count - 1; i >= 0; i--)
                {
                    AbstractPluginDefinition defn = availablePluginsCache[pluginDefinitionType.Name][i];
                    bool hasManager = manager.gameObject.GetComponentInChildren(defn.GetManagerType()) != null;
                    using (new EditorGUI.DisabledScope(hasManager))
                    {
                        if (GUILayout.Button("Enable " + defn.GetReadableName()))
                        {
                            defn.Enable();
                        }
                        if (GUILayout.Button("Learn more about " + defn.GetReadableName() + "... "))
                        {
                            Application.OpenURL(defn.GetURL());
                        }
                    }
                }
            }
        }

        private void EnabledPluginsGUI(Type pluginDefinitionType)
        {
            if (enabledPluginsCache[pluginDefinitionType.Name].Count > 0)
            {
                GUILayout.Label("Currently Enabled");
                for (int i = enabledPluginsCache[pluginDefinitionType.Name].Count - 1; i >= 0; i--)
                {
                    AbstractPluginDefinition defn = enabledPluginsCache[pluginDefinitionType.Name][i];
                    if (GUILayout.Button("Disable " + defn.GetReadableName()))
                    {
                        defn.Disable();
                        enabledPluginsCache[pluginDefinitionType.Name].Remove(defn);
                        availablePluginsCache[pluginDefinitionType.Name].Add(defn);
                    }
                    if (GUILayout.Button("Learn more about " + defn.GetReadableName() + "... "))
                    {
                        Application.OpenURL(defn.GetURL());
                    }
                }
            }
        }

        /// <summary>
        /// Finds all the plugin implementations of a given type and updates their status in
        /// the internal cache:
        /// 
        /// `avaiallbePluginsCache` - Available to be used, but not enabled in the scene.
        /// `enabledPluginsCache` - Available to be used and enabled in the scene.
        /// `supportedPluginsCache` - Known plugins that are not yet available for use (likely has a dependency on a missing asset)
        /// </summary>
        /// <param name="T"></param>
        private void UpdatePluginStatus(Type T)
        {
            List<AbstractPluginDefinition> availablePlugins = new List<AbstractPluginDefinition>();
            List<AbstractPluginDefinition> enabledPlugins = new List<AbstractPluginDefinition>();
            List<AbstractPluginDefinition> supportedPlugins = new List<AbstractPluginDefinition>();

            availablePlugins = new List<AbstractPluginDefinition>();
            enabledPlugins = new List<AbstractPluginDefinition>();
            supportedPlugins = new List<AbstractPluginDefinition>();

            IEnumerable<Type> plugins = ReflectionHelper.GetEnumerableOfType(T);
            foreach (Type pluginType in plugins)
            {
                AbstractPluginDefinition pluginDef = Activator.CreateInstance(pluginType) as AbstractPluginDefinition;
                if (pluginDef.AvailableForUse)
                {
                    AbstractPluginManager pluginManager = (AbstractPluginManager)manager.GetComponentInChildren(pluginDef.GetManagerType());
                    if (pluginManager)
                    {
                        AbstractPluginProfile pluginProfile = pluginManager.m_pluginProfile;
                        if (pluginProfile != null && pluginProfile.GetType().Name == pluginDef.GetProfileTypeName())
                        {
                            enabledPlugins.Add(pluginDef);
                        }
                        else
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

            enabledPluginsCache[T.Name] = enabledPlugins;
            availablePluginsCache[T.Name] = availablePlugins;
            supportedPluginsCache[T.Name] = supportedPlugins;
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