using UnityEngine;
using UnityEditor;
using wizardscode.environment;
using System.Collections.Generic;
using wizardscode.plugin;
using System;

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
            List<ValidationObject> messages = new List<ValidationObject>();
            serializedObject.Update();
            if (pluginProfile.objectReferenceValue != null)
            {
                messages = ((AbstractPluginProfile)pluginProfile.objectReferenceValue).Validate();
            }

            ShowErrors(messages);

            EditorGUILayout.PropertyField(pluginProfile);
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowErrors(List<ValidationObject> messages)
        {
            foreach (ValidationObject msg in messages)
            {
                switch (msg.impact)
                {
                    case ValidationObject.Level.OK:
                        // If it's OK we don't need to report it.
                        break;
                    case ValidationObject.Level.Warning:
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox(msg.message, MessageType.Warning, true);
                        if (msg.resolutionCallback != null)
                        {
                            if (GUILayout.Button("Fix It!"))
                            {
                                msg.resolutionCallback();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        break;
                    case ValidationObject.Level.Error:
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox(msg.message, MessageType.Error, true);
                        if (msg.resolutionCallback != null)
                        {
                            if (GUILayout.Button("Fix It!"))
                            {
                                msg.resolutionCallback();
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                        break;
                }
            }
        }
    }
}