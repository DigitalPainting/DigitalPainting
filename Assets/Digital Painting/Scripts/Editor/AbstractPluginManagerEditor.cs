using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WizardsCode.Plugin;
using WizardsCode.Utility;

namespace WizardsCode.Editor
{
    public abstract class AbstractPluginManagerEditor : UnityEditor.Editor
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
