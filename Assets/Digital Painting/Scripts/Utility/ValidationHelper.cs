using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static wizardscode.utility.ValidationHelper;

namespace wizardscode.utility
{
    public class ValidationHelper
    {
        public delegate void ProfileCallback();

#if UNITY_EDITOR
        static bool showErrors = true;
        static bool showWarnings = true;
        static bool showOk = false;

        public static void ShowValidationResults(List<ValidationResult> messages)
        {
            if (messages.Count == 0)
            {
                return;
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            List<ValidationResult> msgs = messages.Where(x => x.impact == ValidationResult.Level.Error).ToList();
            showErrors = EditorGUILayout.Foldout(showErrors, "Errors: " + msgs.Count());
            if (showErrors)
            {
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in msgs)
                {

                    ValidationResultGUI(msg, MessageType.Error);
                }
                EditorGUI.indentLevel--;
            }

            msgs = messages.Where(x => x.impact == ValidationResult.Level.Warning).ToList();
            showWarnings = EditorGUILayout.Foldout(showWarnings, "Warnings: " + msgs.Count());
            if (showWarnings)
            {
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in msgs)
                {
                    ValidationResultGUI(msg, MessageType.Warning);
                }
                EditorGUI.indentLevel--;
            }

            msgs = messages.Where(x => x.impact == ValidationResult.Level.OK).ToList();
            showOk = EditorGUILayout.Foldout(showOk, "OK: " + msgs.Count());
            if (showOk)
            {
                EditorGUI.indentLevel++;
                foreach (ValidationResult msg in msgs)
                {
                    ValidationResultGUI(msg, MessageType.None);
                }
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }

        private static void ValidationResultGUI(ValidationResult msg, MessageType messageType)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.HelpBox(msg.message, messageType, true);
            if (msg.resolutionCallback != null)
            {
                if (GUILayout.Button("Fix It!"))
                {
                    msg.resolutionCallback();
                }
            }
            EditorGUILayout.EndHorizontal();
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
        /// <param name="impact">The importance of the result from OK to Error.</param>
        public ValidationResult(string message, Level impact)
        {
            this.message = message;
            this.impact = impact;
            this.resolutionCallback = null;
        }

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="message">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        /// <param name="callbackToFix">A callback method that will allow the result to be corrected if possible.</param>
        public ValidationResult(string message, Level impact, ProfileCallback callbackToFix)
        {
            this.message = message;
            this.impact = impact;
            this.resolutionCallback = callbackToFix;
        }
    }
}