using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using wizardscode.extension;
using wizardscode.plugin;

namespace wizardscode.validation
{
    /// <summary>
    /// A test that can be executed in order to validate that the DigitalPainting system and/or its plugins are setup correctly.
    /// Each ValidationTest tests one specific requirement.
    /// </summary>
    public abstract class ValidationTest<T> where T : AbstractPluginManager
    {
        private AbstractPluginManager m_manager;
        internal static ValidationResultCollection ResultCollection = new ValidationResultCollection();

        internal abstract Type ProfileType { get; }

        public abstract ValidationTest<T> Instance { get; }

        public ValidationResultCollection Validate(Type validationTest, AbstractPluginManager pluginInstanceManager)
        {
            ResultCollection = new ValidationResultCollection();
            ValidationResult result;
            
            // Is plugin enabled, If not we don't need to test it
            if (pluginInstanceManager == null)
            {
                return ResultCollection;
            }

            if (!InitialCustomValidations())
            {
                return ResultCollection;
            }

            // Is a plugin profile provided?
            if (pluginInstanceManager.Profile == null)
            {
                result = ResultCollection.GetOrCreate(pluginInstanceManager.GetType().Name.Prettify() + " - Missing Profile", validationTest.Name);
                result.Message = "You need to provide a plugin profile for " + pluginInstanceManager.GetType().Name.BreakCamelCase();
                result.ReportingTest.Add(validationTest.Name);
                result.impact = ValidationResult.Level.Error;
                result.RemoveCallbacks();
                ResultCollection.AddOrUpdate(result, validationTest.Name);

                return ResultCollection;
            }

            if (!ProfileType.Name.EndsWith(pluginInstanceManager.Profile.GetType().Name)) {
                return ResultCollection;
            }
            
            if (!PreFieldCustomValidations())
            {
                return ResultCollection;
            }

            //Validate SettingSO fields
            IEnumerable<FieldInfo> fields = pluginInstanceManager.Profile.GetType().GetFields()
                .Where(field => field.FieldType.IsSubclassOf(typeof(AbstractSettingSO)));
            ValidateFields(validationTest, fields, pluginInstanceManager);
            

            if (!PostFieldCustomValidations())
            {
                return ResultCollection;
            }

            return ResultCollection;
        }

        private void ValidateFields(Type validationTest, IEnumerable<FieldInfo> fields, AbstractPluginManager pluginInstanceManager)
        {
            ValidationResult result;

            foreach (FieldInfo field in fields)
            {
                AbstractSettingSO fieldInstance = field.GetValue(pluginInstanceManager.Profile) as AbstractSettingSO;
                if (fieldInstance == null)
                {
                    AddOrUpdateAsError(field.Name, "Must provide a Setting Scriptable Object");
                    return;
                }

                ResultCollection.Remove(field.Name);

                if (field.FieldType == typeof(GenericSettingSO<>))
                {
                    ValidateGenericSettingField(field, fieldInstance);
                }

                Type type = field.FieldType;
                // Validate the field according to the SO validation setting.
                result = (ValidationResult)type.InvokeMember("Validate",
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                    null, fieldInstance, new object[] { validationTest });

                ResultCollection.AddOrUpdate(result, validationTest.Name);

                IEnumerable<FieldInfo> childFields = field.FieldType.GetFields()
                    .Where(childField => childField.FieldType.IsSubclassOf(typeof(AbstractSettingSO)));
                ValidateChildFields(validationTest, childFields, fieldInstance);
            }
        }

        private void ValidateGenericSettingField(FieldInfo field, AbstractSettingSO fieldInstance)
        {
            string className = ((GenericSettingSO<T>)fieldInstance).valueClassName;
            string accessorName = ((GenericSettingSO<T>)fieldInstance).valueName;

            if ((className != null && className.Length > 0)
                || (accessorName != null && accessorName.Length > 0))
            {
                IEnumerable<Type> propertyTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                                   from candidate in assembly.GetTypes()
                                                   where candidate.Name == className
                                                   select candidate);
                Type propertyType = propertyTypes.FirstOrDefault();

                if (propertyType == null)
                {
                    string msg = "A property accessor is provided but the class identified, `"
                        + className + "`, cannot be found in the Assembly.";
                    AddOrUpdateAsError(field.Name, msg);
                }
                else
                {
                    PropertyInfo accessorPropertyInfo = propertyType.GetProperty(accessorName);
                    FieldInfo acessorFieldInfo = propertyType.GetField(accessorName);
                    if (accessorPropertyInfo == null && acessorFieldInfo == null)
                    {
                        string msg = "A property accessor is provided but the accessor identified, `"
                            + accessorName + "`, cannot be found in the class specified, `" + className + "`.";
                        AddOrUpdateAsError(field.Name, msg);
                    }
                }
            }
        }

        private void ValidateChildFields(Type validationTest, IEnumerable<FieldInfo> fields, AbstractSettingSO parentFieldInstance)
        {
            ValidationResult result;

            foreach (FieldInfo field in fields)
            {
                AbstractSettingSO fieldInstance = field.GetValue(parentFieldInstance) as AbstractSettingSO;
                if (fieldInstance == null)
                {
                    AddOrUpdateAsError(field.Name, "Must provide a Setting Scriptable Object");
                    return;
                }

                ResultCollection.Remove(field.Name);

                if (field.FieldType == typeof(GenericSettingSO<>))
                {
                    ValidateGenericSettingField(field, fieldInstance);
                }

                Type type = field.FieldType;
                // Validate the field according to the SO validation setting.
                result = (ValidationResult)type.InvokeMember("Validate",
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                    null, fieldInstance, new object[] { validationTest });

                ResultCollection.AddOrUpdate(result, validationTest.Name);

                IEnumerable<FieldInfo> childFields = field.FieldType.GetFields()
                    .Where(childField => childField.FieldType.IsSubclassOf(typeof(AbstractSettingSO)));
                ValidateChildFields(validationTest, childFields, fieldInstance);
            }
        }

        /// <summary>
        /// If a plugin needs to perform any specific validations that are not expressed in the form of setting scriptable
        /// objects it should override this method if these validations are to be done before any other validations.
        /// Any successes or failures should be added to the `Collection`.
        /// </summary>
        /// <returns>True if validations passed. False if one ore more warning or error occurs.</returns>
        internal virtual bool InitialCustomValidations() { return true; }

        /// <summary>
        /// If a plugin needs to perform any specific validations that are not expressed in the form of setting scriptable
        /// objects it should override this method if these validations are to be done after profile validation but before 
        /// field validations. Any successes or failures should be added to the `Collection`.
        /// </summary>
        /// <returns>True if validations passed. False if one ore more warning or error occurs.</returns>
        internal virtual bool PreFieldCustomValidations() { return true; }

        /// <summary>
        /// If a plugin needs to perform any specific validations that are not expressed in the form of setting scriptable
        /// objects it should override this method if these validations are to be done after field validations. 
        /// Any successes or failures should be added to the `Collection`.
        /// </summary>
        /// <returns>True if validations passed. False if one ore more warning or error occurs.</returns>
        internal virtual bool PostFieldCustomValidations() { return true; }

        #region ValidationResult creation methods


        /// <summary>
        /// Set the testName conducted by this ValidationTest to a Pass.
        /// </summary>
        /// <param name="testName">Human readable test name.</param>
        /// <param name="message">Human readable message describing the test status.</param>
        internal void AddOrUpdateAsPass(string testName, string message)
        {
            string reportingTest = this.GetType().Name;
            ValidationResult result = GetResult(testName, message, reportingTest);
            result.impact = ValidationResult.Level.OK;
            ResultCollection.AddOrUpdate(result, reportingTest);
        }


        /// <summary>
        /// Set the testName conducted by this ValidationTest to a Warning.
        /// </summary>
        /// <param name="testName">Human readable test name.</param>
        /// <param name="message">Human readable message describing the test status.</param>
        internal void AddOrUpdateAsWarning(string testName, string message, ResolutionCallback callback = null)
        {
            string reportingTest = this.GetType().Name;
            ValidationResult result = GetResult(testName, message, reportingTest, callback);
            result.impact = ValidationResult.Level.Warning;
            ResultCollection.AddOrUpdate(result, reportingTest);
        }

        /// <summary>
        /// Set the testName conducted by this ValidationTest to an Error.
        /// </summary>
        /// <param name="testName">Human readable test name.</param>
        /// <param name="message">Human readable message describing the test status.</param>
        internal void AddOrUpdateAsError(string testName, string message, ResolutionCallback callback = null)
        {
            string reportingTest = this.GetType().Name;
            ValidationResult result = GetResult(testName, message, reportingTest, callback);
            result.impact = ValidationResult.Level.Error;
            ResultCollection.AddOrUpdate(result, reportingTest);
        }

        private ValidationResult GetResult(string testName, string message, string reportingTest, ResolutionCallback callback = null)
        {
            ValidationResult result = ResultCollection.GetOrCreate(testName, reportingTest);
            result.Message = message;
            result.impact = ValidationResult.Level.Warning;
            result.RemoveCallbacks();
            if (callback != null)
            {
                result.AddCallback(callback);
            }
            return result;
        }
        
        [Obsolete("Use AddOrUpdateAsError instead. This will also replace the subsequent call to AddOrUpdate.")]
        internal ValidationResult GetErrorResult(string testName, string message, string reportingTest, ResolutionCallback callback = null)
        {
            ValidationResult result = GetResult(testName, message, reportingTest, callback);
            result.impact = ValidationResult.Level.Error;
            return result;
        }

        [Obsolete("Use AddOrUpdateAsWarning instead. This will also replace the subsequent call to AddOrUpdate.")]
        internal ValidationResult GetWarningResult(string testName, string message, string reportingTest, ResolutionCallback callback = null)
        {
            ValidationResult result = GetResult(testName, message, reportingTest);
            result.impact = ValidationResult.Level.Warning;
            return result;
        }
        
        [Obsolete("Use AddOrUpdateAsPass instead. This will also replace the subsequent call to AddOrUpdate.")]
        internal ValidationResult GetPassResult(string testName, string message, string reportingTest)
        {
            ValidationResult result = GetResult(testName, message, reportingTest);
            result.impact = ValidationResult.Level.OK;
            return result;
        }
        #endregion
    }
}
