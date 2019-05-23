using UnityEngine;
using UnityEditor;
using wizardscode.environment;
using System.Collections.Generic;
using wizardscode.plugin;
using System;
using wizardscode.utility;

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
            ValidationResultCollection results = new ValidationResultCollection();
            serializedObject.Update();
            if (pluginProfile.objectReferenceValue != null)
            {
                results = ((AbstractPluginProfile)pluginProfile.objectReferenceValue).Validate();
            }

            ValidationHelper.ShowValidationResults(results);

            EditorGUILayout.PropertyField(pluginProfile);
            serializedObject.ApplyModifiedProperties();
        }
    }
}