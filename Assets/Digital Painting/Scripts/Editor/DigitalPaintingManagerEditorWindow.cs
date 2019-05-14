using System;
using UnityEditor;
using UnityEngine;
using wizardscode.digitalpainting;

public class DigitalPaintingManagerEditorWindow : EditorWindow
{
    int selectedTab = 0;
    private GUIStyle m_LinkStyle;
    private EditorConfigScriptableObject m_config;
    private string defaultConfigSavePath = "Assets/Digital Painting Editor Config.asset";

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
            GUILayout.Label("In play mode.");
        }
        else
        {
            selectedTab = GUILayout.Toolbar(selectedTab, new string[] { "Standard", "Advanced", "Experimental", "More..." });
            switch (selectedTab)
            {
                case 0:
                    NotImplementedTabGUI();
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

    private void MoreTabGUI()
    {
        if (LinkLabel("Latest Documentation on GitHub"))
        {
            Application.OpenURL(Config.DocsIndexURL);
        }

        GUILayout.FlexibleSpace();

        GUILayout.Label ("Editor Version: " + Config.version);
    }

    private void ExperimentalTabGUI()
    {
        if (!GameObject.Find(Config.ManagerName))
        {
            if (GUILayout.Button("Add Digital Painting Manager"))
            {
                DigitalPaintingManager manager = Instantiate(Config.ManagerPrefab);
                manager.name = Config.ManagerName;
            }
        }
        else
        {
            GUILayout.Label("Your all set, get to work.");
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
                if (!m_config) { 
                    m_config = ScriptableObject.CreateInstance("EditorConfigScriptableObject") as EditorConfigScriptableObject;
                    m_config.Init();
                    AssetDatabase.CreateAsset(m_config, "Assets/Editor Config.asset");
                }
            } 
            else
            {
                if (m_config.version != EditorConfigScriptableObject.LatestVersion)
                {
                    EditorConfigScriptableObject oldConfig = m_config;
                    m_config = ScriptableObject.CreateInstance("EditorConfigScriptableObject") as EditorConfigScriptableObject;
                    m_config.Upgrade(oldConfig);
                    AssetDatabase.CreateAsset(m_config, defaultConfigSavePath);
                }
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
