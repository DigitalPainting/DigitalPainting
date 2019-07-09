using ScriptableObjectArchitecture;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using WizardsCode.extension;
using WizardsCode.plugin;
using WizardsCode.utility;

namespace WizardsCode.validation
{

    /// <summary>
    /// A GenericSettingsSO defines a desired setting for a 
    /// single value. These are used in the validation system to find the optimal
    /// settings when different plugins demand different settings.
    /// </summary>
    public abstract class GenericSettingSO<T> : AbstractSettingSO<T>
    {
        [Tooltip("The name of the class containing the property or field to set. For example, `QualitySettings`.")]
        public string valueClassName;

        [Tooltip("The name of the property or field to set. For example, `shadowDistance`.")]
        public string valueName;

        public override string TestName
        {
            get
            {
                return "Validate setting value : " + SettingName;
            }
        }

        /// <summary>
        /// Gets the actual value of the setting in the game engine.
        /// </summary>
        protected override T ActualValue {
            get
            {
                if (Accessor == null)
                {
                    throw new Exception("No accessor set and ActualValue getter is not overriden in " + GetType());
                }
                
                object value;
                if (Accessor.MemberType == MemberTypes.Property)
                {
                    value = ((PropertyInfo)Accessor).GetValue(default(QualitySettings));
                }
                else
                {
                    FieldInfo field = (FieldInfo)Accessor;
                    if (field.IsStatic)
                    {
                        value = field.GetValue(null);
                    }
                    else
                    {
                        value = field.GetValue(default(QualitySettings));
                    }
                }

                return (T)value;
            }
            set
            {
                if (Accessor.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)Accessor).SetValue(default(QualitySettings), value);
                }
                else
                {
                    FieldInfo field = (FieldInfo)Accessor;
                    if (field.IsStatic)
                    {
                        field.SetValue(null, value);
                    }
                    else
                    {
                        field.SetValue(default(QualitySettings), value);
                    }
                }
            }
        }

        internal MemberInfo m_Accessor = null;
        internal MemberInfo Accessor
        {
            get
            {
                if (m_Accessor == null)
                {
                    IEnumerable<Type> propertyTypes = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                                                       from candidate in assembly.GetTypes()
                                                       where candidate.Name == valueClassName
                                                       select candidate);
                    Type propertyType = propertyTypes.FirstOrDefault();

                    if (propertyType == null)
                    {
                        return null;
                    }

                    PropertyInfo accessorPropertyInfo = propertyType.GetProperty(valueName);
                    if (accessorPropertyInfo != null)
                    {
                        m_Accessor = accessorPropertyInfo;
                    }
                    else
                    {
                        m_Accessor = propertyType.GetField(valueName);
                    }
                }

                return m_Accessor;
            }
        }

        public override void Fix()
        {
            ActualValue = SuggestedValue;
        }

        internal override ValidationResult ValidateSetting(Type validationTest, AbstractPluginManager pluginManager)
        {
            ValidationResult result = null;
            T value;

            try
            {
                value = ActualValue;
            }
            catch (Exception e)
            {
                result = GetErrorResult(TestName, pluginManager, "Exception validating " + name + "\n" + e.Message + "\n" + e.StackTrace, validationTest.Name);
                result.RemoveCallbacks();
                return result;
            }
            
            if (!object.Equals(value, SuggestedValue))
            {
                result = GetWarningResult(TestName, pluginManager, "The value set is not the same as the suggested value.\n"
                    + "Suggested value = " + SuggestedValue + "\n"
                    + "Actual Value = " + value + "\n"
                    + "This may be OK, in which case click the ignore button.", validationTest.Name);
                return result;
            }

            return GetPassResult(TestName, pluginManager, validationTest.Name);
        }
    }
}
