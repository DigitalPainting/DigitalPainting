using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using wizardscode.plugin;
using wizardscode.validation;
using static wizardscode.utility.ValidationHelper;

namespace wizardscode.utility
{
    public class ValidationHelper
    {

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
            Remove(result.name);
            collection[result.id] = result;
        }

        public void AddOrUpdateAll(ValidationResultCollection results)
        {
            foreach (ValidationResult result in results.collection.Values)
            {
                if (result.impact != ValidationResult.Level.OK)
                {
                    collection[result.id] = result;
                }
            }
        }

        public void Remove(string name)
        {
            collection.Remove(name.GetHashCode());
        }

        public int Count
        {
            get { return collection.Count(); }
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
    public class ValidationTest<T> where T : AbstractPluginManager
    {
        private AbstractPluginManager m_manager;

        private AbstractPluginManager Manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = GameObject.FindObjectOfType<T>(); ;
                }
                return m_manager;
            }
        }
        public ValidationTest<T> Instance => new ValidationTest<T>();

        public ValidationResultCollection Execute()
        {
            ValidationResultCollection localCollection = new ValidationResultCollection();
            IEnumerable<FieldInfo> fields = Manager.Profile.GetType().GetFields()
                .Where(field => field.FieldType.IsSubclassOf(typeof(AbstractSettingSO)));

            foreach (FieldInfo field in fields)
            {
                ValidationResult result;
                object instance = field.GetValue(Manager.Profile);
                if (instance == null)
                {
                    result = localCollection.GetOrCreate(field.Name);
                    result.Message = "Must provide a Setting Scriptable Object";
                    result.impact = ValidationResult.Level.Error;
                }
                else
                {
                    localCollection.Remove(field.Name);

                    Type type = field.FieldType;
                    result = (ValidationResult)type.InvokeMember("Validate",
                        BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                        null, instance, null);
                }
                localCollection.AddOrUpdate(result);
            }

            // FIXME: ensure all tests are moved to the new model
            // localCollection.AddOrUpdateAll(ExecuteOriginal());
            return localCollection;
        }
    }
}