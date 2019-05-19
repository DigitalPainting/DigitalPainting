using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wizardscode.plugin
{
    public abstract class AbstractPluginProfile : ScriptableObject
    {
        public delegate void ProfileCallback();

        /// <summary>
        /// Test to see if the plugin profile is compatible with the current scene. 
        /// </summary>
        /// <returns>A list of strings that describe any problems found. If the list is empty then no errors were found.</returns>
        public virtual List<ValidationObject> Validate()
        {
            return new List<ValidationObject>();
        }
    }
    

    /// <summary>
    /// A ValidationObject captures the results of a validation test.
    /// These can be used to help the designer improve on their scene.
    /// </summary>
    public class ValidationObject
    {
        public enum Level { OK, Warning, Error }
        public Level impact;
        public string message;
        public AbstractPluginProfile.ProfileCallback resolutionCallback;

        /// <summary>
        /// Create a Validation object.
        /// </summary>
        /// <param name="message">A human readable message describing the validation state.</param>
        /// <param name="error">Indicates the level of severity of the message. From OK, meaning all good, to Error meaning it won't work like this.</param>
        public ValidationObject(string message, Level impact, AbstractPluginProfile.ProfileCallback callback)
        {
            this.message = message;
            this.impact = impact;
            this.resolutionCallback = callback;
        }
    }
}
