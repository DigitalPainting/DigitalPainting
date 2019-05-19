using UnityEngine;
using UnityEditor;
using wizardscode.environment;

namespace wizardscode.editor {
    [CustomEditor(typeof(DayNightPluginManager))]
    public class DayNightPluginManagerEditor : Editor
    {
        SerializedProperty pluginProfile;

        void OnEnable()
        {
            pluginProfile = serializedObject.FindProperty("m_pluginProfile");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (pluginProfile.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("You must select a Day Night Plugin Profile", MessageType.Warning, true);
            }
            EditorGUILayout.PropertyField(pluginProfile);
            serializedObject.ApplyModifiedProperties();
        }
    }
}