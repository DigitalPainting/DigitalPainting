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

        public override string TestName
        {
            get
            {
                if (IsPrefabSetting)
                {
                    return "Validate prefab setup : " + SettingName;
                }
                else
                {
                    return "Validate setting value : " + SettingName;
                }
            }
        }

        private bool IsPrefabSetting
        {
            get { return (SuggestedValue as UnityEngine.Object) != null 
                    && PrefabUtility.IsPartOfAnyPrefab(SuggestedValue as UnityEngine.Object); }
        }

        /// <summary>
        /// Gets the actual value of the setting in the game engine.
        /// </summary>
        protected virtual T ActualValue {
            get
            {
                if (Accessor == null)
                {
                    throw new Exception("No accessor set and ActualValue getter is no overriden");
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
                if (Accessor == null)
                {
                    throw new Exception("No accessor set and ActualValue setter is no overriden");
                }

                if (Accessor.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo)Accessor).SetValue(default(QualitySettings), value);
                }
                else
                {
                    ((FieldInfo)Accessor).SetValue(default(QualitySettings), value);
                }
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
            if (IsPrefabSetting)
            {
                throw new Exception("The suggested value is a prefab, you need to override the Fix method in your *SettingSO to fix the failed test.");
            }
            else
            {
                ActualValue = SuggestedValue;
            }
        }

        private void InstantiatePrefab()
        {
            PrefabUtility.InstantiatePrefab(SuggestedValue as UnityEngine.Object);
        }

        /// <summary>
        /// Get an instance object that was created by the Suggested Value (assuming it is a Prefab).
        /// </summary>
        /// <returns></returns>
        internal GameObject GetFirstInstanceInScene()
        {
            GameObject go = null;
            UnityEngine.Object[] gos = GameObject.FindObjectsOfType(SuggestedValue.GetType());
            foreach (UnityEngine.Object candidate in gos)
            {
                if (UnityEngine.Object.ReferenceEquals(PrefabUtility.GetCorrespondingObjectFromOriginalSource(candidate), SuggestedValue))
                {
                    if (candidate is Component)
                    {
                        go = ((Component)candidate).gameObject;
                    } else
                    {
                        go = (GameObject)candidate;
                    }
                    break;
                }
            }

            return go;
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

            if (IsPrefabSetting)
            {
                GameObject go = GetFirstInstanceInScene();

                ResolutionCallback callback = new ResolutionCallback(InstantiatePrefab);
                if (AddToScene)
                {
                    if (go == null)
                    {
                        result = GetErrorResult(TestName, "The object required doesn't currently exist in the scene", validationTest.Name);
                        List<ResolutionCallback> callbacks = new List<ResolutionCallback>();
                        callbacks.Add(callback);
                        result.Callbacks = callbacks;
                        return result;
                    }
                }

                GameObject actualGO;
                if (value is Component)
                {
                    actualGO = (value as Component).gameObject;
                }
                else
                {
                    actualGO = value as GameObject;
                }

                if (actualGO as UnityEngine.Object == null || !ReferenceEquals(actualGO, go))
                {
                    result = GetWarningResult(TestName, "The value set is not an object in the scene that has been instantiated from the suggested value prefab. This may be OK, in which case click the ignore button.", validationTest.Name);
                    result.RemoveCallbacks();
                    AddDefaultFixCallback(result);
                    return result;
                }
            }
            else
            {
                if (!object.Equals(value, SuggestedValue))
                {
                    result = GetWarningResult(TestName, "The value set is not the same as the suggested value.\n"
                        + "Suggested value = " + SuggestedValue + "\n"
                        + "Actual Value = " + value + "\n"
                        + "This may be OK, in which case click the ignore button.", validationTest.Name);
                    return result;
                }
            }

            return GetPassResult(TestName, validationTest.Name);
        }
    }
}
