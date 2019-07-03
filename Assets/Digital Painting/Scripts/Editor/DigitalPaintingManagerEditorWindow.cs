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
using WizardsCode.digitalpainting;
using WizardsCode.extension;
using WizardsCode.plugin;
using WizardsCode.validation;

namespace WizardsCode.editor
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

        Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>> enabledPluginsCache = new Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>>();
        Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>> availablePluginsCache = new Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>>();
        Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>> supportedPluginsCache = new Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>>();
        private float frequencyOfPluginRefresh = 5;
        private DateTime timeOfNextPluginRefresh = DateTime.Now;
        private string configAssetPath = "Assets/Digital Painting Editor Config.asset";
        private string iconAssetPath = "DigitalPainting/Assets/Digital Painting/icons/";
        private string iconOKFile = "Silk/accept.png";
        private string iconWarningFile = "Silk/bug.png";
        private string iconErrorFile = "Silk/bug_error.png";

        private Texture2D iconOK;
        private Texture2D iconWarning;
        private Texture2D iconError;

        protected static ValidationResultCollection Validations = new ValidationResultCollection();
        
        private void OnEnable()
        {
            iconOK = EditorGUIUtility.Load("Assets/" + iconAssetPath + iconOKFile) as Texture2D;
            iconWarning = EditorGUIUtility.Load("Assets/" + iconAssetPath + iconWarningFile) as Texture2D;
            iconError = EditorGUIUtility.Load("Assets/" + iconAssetPath + iconErrorFile) as Texture2D;

            titleContent = new GUIContent("Digital Painting");
            titleContent.image = iconOK;
        }

        [MenuItem("Window/Wizards Code/Open Digital Painting Manager")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(DigitalPaintingManagerEditorWindow));
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
                Validate();

                GameObject go = GameObject.Find(Config.ManagerName);
                if (go)
                {
                    manager = go.GetComponent<DigitalPaintingManager>();
                }

                ShowStatusSummaryGUI();

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
            switch (result.impact)
            {
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
            }
            else
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

            if (GUILayout.Button("Ping Plugin Manager"))
            {
                EditorGUIUtility.PingObject(result.PluginManager);
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
                    }
                    else
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
        /// </summary>
        //were found.</returns>
        public virtual void Validate()
        {
            timeToNextValidation -= Time.deltaTime;
            if (timeToNextValidation <= 0)
            {
                Validations = new ValidationResultCollection();

                AbstractPluginManager[] pluginManagers = GameObject.FindObjectsOfType<AbstractPluginManager>();
                for (int i = 0; i < pluginManagers.Length; i++)
                {
                    Type genericType = typeof(ValidationTest<>).MakeGenericType(new Type[] { pluginManagers[i].GetType() });
                    IEnumerable<Type> validationTypes = Assembly.GetAssembly(genericType).GetTypes()
                                        .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(genericType));
                    foreach (Type type in validationTypes)
                    {
                        var test = Activator.CreateInstance(type);
                        MethodInfo method = type.GetMethod("Validate");
                        ValidationResultCollection results = (ValidationResultCollection)method.Invoke(test, new object[] { type, pluginManagers[i] });

                        Validations.AddOrUpdateAll(results);
                    }
                }
                timeToNextValidation = frequencyOfValidation;
            }
        }

        private void StandardTabGUI()
        {
            UpdateAllPluginDefinitions();

            Array categories = Enum.GetValues(typeof(AbstractPluginDefinition.PluginCategory));

            foreach (AbstractPluginDefinition.PluginCategory category in categories)
            {
                PluginSelectionGUI(category);
            }
        }

        /// <summary>
        /// Shows a small panel with a summary of errors. Intended for inclusion at the top of
        /// each GUI panel.
        /// </summary>
        private void ShowStatusSummaryGUI()
        {
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
                titleContent.image = iconError;
            } else if (notIgnoredWarningCount > 0)
            {
                type = MessageType.Warning;
                result = Validations.GetHighestPriorityErrorOrWarning(ignoredTests);
                titleContent.image = iconWarning;
            } else
            {
                title = "Everything looks to be set up correctly.\n";
                title += (totalErrorCount - notIgnoredErrorCount) + " ignored errors.\n";
                title += (totalWarningCount - notIgnoredWarningCount) + " ignored warnings.\n";
                titleContent.image = iconOK;
                result = null;
            }
            
            EditorGUILayout.HelpBox(title, type);

            if (result != null)
            {
                ValidationResultGUI(result);
            }
        }

        /// <summary>
        /// Updates the list of plugins known to the system and updates their status .
        /// (available, enabled or supported).
        /// 
        /// `avaiallbePluginsCache` - Available to be used, but not enabled in the scene.
        /// `enabledPluginsCache` - Available to be used and enabled in the scene.
        /// `supportedPluginsCache` - Known plugins that are not yet available for use (likely has a dependency on a missing asset)
        /// </summary>
        private void UpdateAllPluginDefinitions()
        {
            if (DateTime.Now >= timeOfNextPluginRefresh)
            {
                availablePluginsCache.Clear();
                enabledPluginsCache.Clear();
                supportedPluginsCache.Clear();

                IEnumerable<Type> types = ReflectionHelper.GetClassesWithBaseType<AbstractPluginDefinition>();
                foreach (Type type in types)
                {
                    UpdatePluginStatus(type);
                }
                timeOfNextPluginRefresh = DateTime.Now.AddSeconds(frequencyOfPluginRefresh);
            }
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
        }

        /// <summary>
        /// Find all the plugins of a a given type known to the system and display
        /// a UI for working with them.
        /// </summary>
        /// <param name="category">The AbstractPluginDefinition.PluginCategory of plugins to display in the GUI</param>
        private void PluginSelectionGUI (AbstractPluginDefinition.PluginCategory category)
        {
            GUILayout.BeginVertical("Box");
            string categoryName = category.ToString().Prettify();

            GUILayout.Label(categoryName + " Plugins", EditorStyles.boldLabel);

            GUILayout.BeginVertical("Box");

            try
            {
                EnabledPluginsGUI(category);
            }
            catch (KeyNotFoundException e)
            {
                // That's just fine, no enabled plugins in this category
            }

            try
            {
                AvailablePluginsGUI(category);
            }
                catch (KeyNotFoundException e)
            {
                // That's just fine, no enabled plugins in this category
            }

            try
            {
                SupportedPluginsGUI(category);
            }
            catch (KeyNotFoundException e)
            {
                // That's just fine, no enabled plugins in this category
            }

            GUILayout.EndVertical();

            GUILayout.EndVertical();
        }

        private void SupportedPluginsGUI(AbstractPluginDefinition.PluginCategory category)
        {
            List<AbstractPluginDefinition> plugins;
            try
            {
                plugins = supportedPluginsCache[category];
            if (plugins.Count > 0)
            {
                EditorGUILayout.BeginVertical("Box");


                float columnWidth = EditorGUIUtility.currentViewWidth / 3;
                EditorGUILayout.LabelField("Supported but not installed");
                foreach (AbstractPluginDefinition defn in supportedPluginsCache[category])
                {
                    if (GUILayout.Button("Learn more about " + defn.GetReadableName() + "... "))
                    {
                        Application.OpenURL(defn.GetURL());
                    }
                }

                EditorGUILayout.EndVertical();
                }
            }
            catch (KeyNotFoundException e)
            {
                // That's just fine, no enabled plugins in this category
            }
        }

        private void AvailablePluginsGUI(AbstractPluginDefinition.PluginCategory category)
        {
            List<AbstractPluginDefinition> plugins;
            try
            {
                plugins = availablePluginsCache[category];
                if (plugins.Count > 0)
                {
                    if (plugins.Count > 0)
                    {
                        EditorGUILayout.LabelField("Available for use");
                        for (int i = availablePluginsCache[category].Count - 1; i >= 0; i--)
                        {
                            AbstractPluginDefinition defn = availablePluginsCache[category][i];

                            EditorGUILayout.BeginHorizontal();
                            EditorGUI.indentLevel++;

                            float columnWidth = EditorGUIUtility.currentViewWidth / 3;
                            EditorGUILayout.LabelField(defn.GetReadableName(), GUILayout.Width(columnWidth));

                            bool hasManager = manager.gameObject.GetComponentInChildren(defn.GetManagerType()) != null;
                            if (!hasManager || defn.MultipleAllowed)
                            {
                                if (GUILayout.Button("Enable"))
                                {
                                    defn.Enable();
                                    timeOfNextPluginRefresh = DateTime.Now;
                                    UpdateAllPluginDefinitions();
                                }
                                if (GUILayout.Button("Learn more"))
                                {
                                    Application.OpenURL(defn.GetURL());
                                }
                            }

                            EditorGUI.indentLevel--;
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }
            catch (KeyNotFoundException e)
            {
                // That's just fine, no enabled plugins in this category
            }
        }

        private void EnabledPluginsGUI(AbstractPluginDefinition.PluginCategory category)
        {
            if (enabledPluginsCache[category].Count > 0)
            {
                GUILayout.Label("Currently Enabled");
                for (int i = enabledPluginsCache[category].Count - 1; i >= 0; i--)
                {
                    AbstractPluginDefinition defn = enabledPluginsCache[category][i];

                    Component pluginManager = manager.gameObject.GetComponentsInChildren(defn.GetManagerType())[i];

                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.indentLevel++;

                    float columnWidth = EditorGUIUtility.currentViewWidth / 3;
                    EditorGUILayout.LabelField(pluginManager.name + " (" + defn.GetReadableName() + ")", GUILayout.Width(columnWidth));

                    if (GUILayout.Button("Disable"))
                    {
                        DestroyImmediate(pluginManager.gameObject);
                        timeOfNextPluginRefresh = DateTime.Now;
                        UpdateAllPluginDefinitions();
                    }
                    if (GUILayout.Button("Learn more"))
                    {
                        Application.OpenURL(defn.GetURL());
                    }

                    EditorGUI.indentLevel--;
                    EditorGUILayout.EndHorizontal();
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
            // Iterate over all the known plugins of this type
            IEnumerable<Type> plugins = ReflectionHelper.GetEnumerableOfType(T);
            foreach (Type pluginType in plugins)
            {
                // If any dependencies are present then provide enable/disable buttons, otherwise provide more info only
                AbstractPluginDefinition pluginDef = Activator.CreateInstance(pluginType) as AbstractPluginDefinition;
                if (pluginDef.AvailableForUse)
                {
                    bool isAvailable = true;
                    // See if there are any existing instances of this plugin in the scene
                    Component[] components = manager.GetComponentsInChildren(pluginDef.GetManagerType());
                    AbstractPluginManager[] pluginManagers = Array.ConvertAll(components, item => (AbstractPluginManager)item);

                    if (pluginManagers != null && pluginManagers.Count() != 0)
                    {
                        for (int i = 0; i < pluginManagers.Count(); i++)
                        {
                            AbstractPluginProfile pluginProfile = pluginManagers[i].m_pluginProfile;
                            if (pluginProfile != null && pluginDef.GetProfileTypeName().EndsWith(pluginProfile.GetType().Name))
                            {
                                AddToCache(enabledPluginsCache, pluginDef);
                                if (!pluginDef.MultipleAllowed)
                                {
                                    isAvailable = false;
                                }
                            }
                        }
                    }
                    
                    if (isAvailable) {
                        // There are no existing managers in the scene or multiple are allowed, show enable options
                        AddToCache(availablePluginsCache, pluginDef);
                    }
                }
                else
                {
                    AddToCache(supportedPluginsCache, pluginDef);
                }
            }
        }

        /// <summary>
        /// Add a plugin definition to a cache.
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="defn"></param>
        private void AddToCache(Dictionary<AbstractPluginDefinition.PluginCategory, List<AbstractPluginDefinition>> cache, AbstractPluginDefinition defn) {
            try
            {
                cache[defn.GetCategory()].Add(defn);
            } catch (KeyNotFoundException e)
            {
                List<AbstractPluginDefinition> list = new List<AbstractPluginDefinition>();
                list.Add(defn);
                cache.Add(defn.GetCategory(), list);
            }
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