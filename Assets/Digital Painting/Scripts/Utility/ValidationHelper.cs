using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static wizardscode.utility.ValidationHelper;

namespace wizardscode.utility
{
    public class ValidationHelper
    {
        public delegate void ProfileCallback();

#if UNITY_EDITOR
        public static void ShowValidationResults(List<ValidationResult> messages)
        {
            if (messages.Count == 0)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            foreach (ValidationResult msg in messages)
            {
                switch (msg.impact)
                {
                    case ValidationResult.Level.OK:
                        // If it's OK we don't need to report it.
                        break;
                    case ValidationResult.Level.Warning:
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
                    case ValidationResult.Level.Error:
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
            EditorGUILayout.EndVertical();
        }
#endif
    }
    
    /// <summary>
    /// A ValidationResult captures the results of a validation test.
    /// These can be used to help the designer improve on their scene.
    /// </summary>
    public class ValidationResult
    {
        public enum Level { OK, Warning, Error }
        public Level impact;
        public string message;
        public ProfileCallback resolutionCallback;

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="message">A human readable message describing the validation state.</param>
        /// <param name="error">Indicates the level of severity of the message. From OK, meaning all good, to Error meaning it won't work like this.</param>
        public ValidationResult(string message, Level impact, ProfileCallback callback = null)
        {
            this.message = message;
            this.impact = impact;
            this.resolutionCallback = callback;
        }
    }
}