using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WizardsCode.plugin;
using WizardsCode.utility;

namespace WizardsCode.editor
{
    public abstract class AbstractPluginManagerEditor : Editor
    {
        internal SerializedProperty pluginProfile;

        void OnEnable()
        {
            pluginProfile = serializedObject.FindProperty("m_pluginProfile");
        }

        public override void OnInspectorGUI()
        {
            /*
            ValidationResultCollection results = new ValidationResultCollection();
            serializedObject.Update();
            if (pluginProfile.objectReferenceValue != null)
            {
                results = ((AbstractPluginProfile)pluginProfile.objectReferenceValue).Validate();
            }

            ValidationHelper.ShowValidationResults(results);
            */

            EditorGUILayout.PropertyField(pluginProfile);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
