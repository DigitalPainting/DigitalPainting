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
        public static ValidationResultCollection Validations = new ValidationResultCollection();

#if UNITY_EDITOR
        static bool showErrors = true;
        static bool showWarnings = true;
        static bool showOk = false;

        public static void ShowValidationResults(ValidationResultCollection messages)
        {
            if (messages.Count == 0)
            {
                return;
            }

            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical();

            List<ValidationResult> msgs = messages.ErrorList;
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

            msgs = messages.WarningList;
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

            msgs = messages.OKList;
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
            if (msg.ignore)
            {
                return;
            }

            EditorGUILayout.BeginVertical();
            EditorGUILayout.HelpBox(msg.message, messageType, true);

            if (msg.resolutionCallback != null)
            {
                if (GUILayout.Button("Fix It!"))
                {
                    msg.resolutionCallback();
                }
            }
            msg.ignore = EditorGUILayout.Toggle("Ignore", msg.ignore);
            EditorGUILayout.EndVertical();
        }
#endif
    }

    public class ValidationResultCollection
    {
        Dictionary<int, ValidationResult> collection = new Dictionary<int, ValidationResult>();


        /// <summary>
        /// Get or create a ValidationResult for a named validation test.
        /// </summary>
        /// <param name="name">The name of the validation test.</param>
        /// <returns>An existing ValidationResult if the test has already been run, or a new validation result with an untested state.</returns>
        public ValidationResult GetOrCreate(string name)
        {
            ValidationResult result;
            if (!collection.TryGetValue(name.GetHashCode(), out result))
            {
                result = new ValidationResult(name);
                AddOrUpdate(result);
            }
            return result;
        }

        public void AddOrUpdate(ValidationResult result)
        {
            collection[result.id] = result;
        }

        public int Count
        {
            get { return collection.Count(); }
        }

        public int CountOK
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.OK); }
        }

        public int CountWarning
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Warning); }
        }

        public int CountError
        {
            get { return collection.Values.Count(x => x.impact == ValidationResult.Level.Error); }
        }

        public List<ValidationResult> ErrorList {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.Error).ToList(); }
        }

        public List<ValidationResult> WarningList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.Warning).ToList(); }
        }

        public List<ValidationResult> OKList
        {
            get { return collection.Values.Where(x => x.impact == ValidationResult.Level.OK).ToList(); }
        }
    }

    /// <summary>
    /// A test that can be executed in order to validate that the DigitalPainting system and/or its plugins are setup correctly.
    /// Each ValidationTest tests one specific requirement.
    /// </summary>
    public interface IValidationTest
    {
        IValidationTest Instance { get; }

        ValidationResult Execute();
    }
    
    /// <summary>
    /// A ValidationResult captures the results of a validation test.
    /// These can be used to help the designer improve on their scene.
    /// </summary>
    public class ValidationResult
    {
        public enum Level { OK, Warning, Error, Untested }

        public int id;
        public Level impact;
        public string message;
        public ProfileCallback resolutionCallback;
        public bool ignore = false;

        /// <summary>
        /// Create a Validation object in an untested state.
        /// </summary>
        /// <param name="message">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        internal ValidationResult(string message) : this(message, Level.Untested, null)
        {
        }

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="message">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        internal ValidationResult(string message, Level impact) : this(message, impact, null)
        {
        }

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="message">A human readable message describing the validation state.</param>
        /// <param name="impact">The importance of the result from OK to Error.</param>
        /// <param name="callbackToFix">A callback method that will allow the result to be corrected if possible.</param>
        internal ValidationResult(string message, Level impact, ProfileCallback callbackToFix)
        {
            this.id = message.GetHashCode();
            this.message = message;
            this.impact = impact;
            this.resolutionCallback = callbackToFix;
        }
    }
}