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
using wizardscode.extension;
using wizardscode.utility;

namespace wizardscode.validation
{
    public abstract class GenericSettingSO<T> : AbstractSettingSO 
    {
        /// <summary>
        /// A GenericSettingsSO defines a desired setting for a 
        /// single value. These are used in the validation system to find the optimal
        /// settings when different plugins demand different settings.
        /// </summary>
        [Tooltip("The suggested value for the setting. Other values may work, but if in doubt use this setting.")]
        public T SuggestedValue;

        [Tooltip("The event to fire whenever the value is correctly set. Note that this will only ever be fired in the editor and thus listeners must be active in the editor.")]
        public GameEventBase<T> OnSetEvent;

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
        protected virtual T ActualValue {
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
                    value = ((FieldInfo)m_Accessor).GetValue(default(QualitySettings));
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
                    ((FieldInfo)Accessor).SetValue(default(QualitySettings), value);
                }

                FireOnSetEvent();
            }
        }

        private void FireOnSetEvent()
        {
            if (OnSetEvent != null)
            {
                OnSetEvent.Raise(ActualValue);
            }
        }

        MemberInfo m_Accessor = null;
        private MemberInfo Accessor
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

        public override ValidationResult Validate(Type validationTest)
        {
            ValidationResult result;
            if (!Nullable)
            { 
                string testName = "Suggested Value (" + validationTest.Name.BreakCamelCase() + ")";
                if (SuggestedValue is UnityEngine.Object)
                {
                    if (SuggestedValue as UnityEngine.Object == null)
                    {
                        result = GetErrorResult(testName, "Suggested value cannot be null.", validationTest.Name);
                        result.ReportingTest.Add(validationTest.Name);
                        return result;
                    } else
                    {
                        result = GetPassResult(testName, validationTest.Name);
                    }
                } else if (SuggestedValue == null)
                {
                    result = GetErrorResult(testName, "Suggested value cannot be null.", validationTest.Name);
                    result.ReportingTest.Add(validationTest.Name);
                    return result;
                }
                else
                {
                    result = GetPassResult(testName, validationTest.Name);
                }
            }

            return ValidateSetting(validationTest);
        }

        public override void Fix()
        {
            ActualValue = SuggestedValue;
        }

        internal override ValidationResult ValidateSetting(Type validationTest)
        {
            ValidationResult result = null;
            T value;

            try
            {
                value = ActualValue;
            }
            catch (Exception e)
            {
                result = GetErrorResult(TestName, e.Message, validationTest.Name);
                result.RemoveCallbacks();
                return result;
            }
            
            if (!object.Equals(value, SuggestedValue))
            {
                result = GetWarningResult(TestName, "The value set is not the same as the suggested value.\n"
                    + "Suggested value = " + SuggestedValue + "\n"
                    + "Actual Value = " + value + "\n"
                    + "This may be OK, in which case click the ignore button.", validationTest.Name);
                return result;
            }

            return GetPassResult(TestName, validationTest.Name);
        }
    }
}
