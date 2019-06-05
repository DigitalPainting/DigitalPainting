using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using wizardscode.digitalpainting;
using wizardscode.extension;
using wizardscode.plugin;
using wizardscode.utility;

namespace wizardscode.validation
{
    /// <summary>
    /// A test that can be executed in order to validate that the DigitalPainting system and/or its plugins are setup correctly.
    /// Each ValidationTest tests one specific requirement.
    /// </summary>
    public abstract class ValidationTest<T> where T : AbstractPluginManager
    {
        private AbstractPluginManager m_manager;
        static ValidationResultCollection Collection = new ValidationResultCollection();

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

        internal abstract string ProfileType { get; }

        public abstract ValidationTest<T> Instance { get; }

        public ValidationResultCollection Validate(Type validationTest)
        {
            Collection = new ValidationResultCollection();
            ValidationResult result;
            
            // Is plugin enabled, If not we don't need to test it
            if (Manager == null)
            {
                return Collection;
            }

            // Is a plugin profile provided?
            if(Manager.Profile == null)
            {
                result = Collection.GetOrCreate(Manager.GetType().Name.BreakCamelCase() + " - Missing Profile", validationTest.Name);
                result.Message = "You need to provide a plugin profile for " + Manager.GetType().Name.BreakCamelCase();
                result.ReportingTest.Add(validationTest.Name);
                result.impact = ValidationResult.Level.Error;
                result.RemoveCallbacks();
                Collection.AddOrUpdate(result, validationTest.Name);

                return Collection;
            }

            if (Manager.Profile.GetType().Name != ProfileType) {
                return Collection;
            }

            // Get all the SettingSO fields
            IEnumerable<FieldInfo> fields = Manager.Profile.GetType().GetFields()
                .Where(field => field.FieldType.IsSubclassOf(typeof(AbstractSettingSO)));
                
            // Validate the fields
            foreach (FieldInfo field in fields)
            {
                AbstractSettingSO fieldInstance = field.GetValue(Manager.Profile) as AbstractSettingSO;
                if (fieldInstance == null)
                {
                    result = Collection.GetOrCreate(field.Name, validationTest.Name);
                    result.Message = "Must provide a Setting Scriptable Object";
                    result.impact = ValidationResult.Level.Error;
                    Collection.AddOrUpdate(result, validationTest.Name);
                    return Collection;
                }

                /**
                Type[] genericTypes = fieldInstance.GetType().BaseType.GetGenericArguments();
                if (genericTypes.Count() == 0)
                {
                    return Collection;
                }
    */

                Collection.Remove(field.Name);

                // if a PropertyAccessorName is provided ensure it exists
                if (field.FieldType == typeof(GenericSettingSO<>))
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
                            result = Collection.GetOrCreate(field.Name, validationTest.Name);
                            result.Message = "A property accessor is provided but the class identified, `"
                                + className + "`, cannot be found in the Assembly.";
                            result.impact = ValidationResult.Level.Error;
                            Collection.AddOrUpdate(result, validationTest.Name);
                        }
                        else
                        {
                            PropertyInfo accessorPropertyInfo = propertyType.GetProperty(accessorName);
                            FieldInfo acessorFieldInfo = propertyType.GetField(accessorName);
                            if (accessorPropertyInfo == null && acessorFieldInfo == null)
                            {
                                result = Collection.GetOrCreate(field.Name, validationTest.Name);
                                result.Message = "A property accessor is provided but the accessor identified, `"
                                    + accessorName + "`, cannot be found in the class specified, `" + className + "`.";
                                result.impact = ValidationResult.Level.Error;
                                Collection.AddOrUpdate(result, validationTest.Name);
                            }
                        }
                    }
                }
                    
                Type type = field.FieldType;
                // Validate the field according to the SO validation setting.
                result = (ValidationResult)type.InvokeMember("Validate",
                    BindingFlags.InvokeMethod | BindingFlags.Instance | BindingFlags.Public,
                    null, fieldInstance, new object[] { validationTest });
                
                Collection.AddOrUpdate(result, validationTest.Name);
            }

            return Collection;
        }
    }
}
